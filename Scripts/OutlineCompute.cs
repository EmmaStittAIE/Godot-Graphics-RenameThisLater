using Godot;
using System;

public partial class OutlineCompute : Node
{
    [Export]
    public Image shaderImage = new Image();

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        RenderingDevice renderingDevice = RenderingServer.CreateLocalRenderingDevice();

        // Load shader
        RDShaderFile shaderFile = GD.Load<RDShaderFile>("res://Shaders/OutlineCompute.glsl");
        RDShaderSpirV shaderSpirV = shaderFile.GetSpirV();
        Rid shaderID = renderingDevice.ShaderCreateFromSpirV(shaderSpirV);

        // Prepare data
        uint texWidth = 8;
        uint texHeight = 8;

        Image inputImage = GD.Load<Texture2D>("res://Textures/kirbo.png").GetImage();
        inputImage.Convert(Image.Format.Rgbaf);

        RDTextureFormat inputTexFormat = new RDTextureFormat()
        {
            Width = (uint)inputImage.GetWidth(),
            Height = (uint)inputImage.GetHeight(),
            Format = RenderingDevice.DataFormat.R32G32B32A32Sfloat,
            UsageBits = RenderingDevice.TextureUsageBits.CanUpdateBit
                      | RenderingDevice.TextureUsageBits.SamplingBit
                      | RenderingDevice.TextureUsageBits.CanCopyFromBit
        };

        RDTextureView inputTexView = new RDTextureView();

        RDTextureFormat outputTexFormat = new RDTextureFormat()
        {
            Width = texWidth,
            Height = texHeight,
            Format = RenderingDevice.DataFormat.R32G32B32A32Sfloat,
            UsageBits = RenderingDevice.TextureUsageBits.CanUpdateBit
                      | RenderingDevice.TextureUsageBits.StorageBit
                      | RenderingDevice.TextureUsageBits.CanCopyFromBit
        };

        RDTextureView outputTexView = new RDTextureView();

        RDSamplerState samplerState = new RDSamplerState();
        Rid samplerID = renderingDevice.SamplerCreate(samplerState);

        Image outputImage = Image.Create((int)texWidth, (int)texHeight, false, Image.Format.Rgbaf);

        // Create image buffer
        Rid inputTexID = renderingDevice.TextureCreate(inputTexFormat, inputTexView, Variant.From(inputImage.GetData()).AsGodotArray<byte[]>());
        Rid outputTexID = renderingDevice.TextureCreate(outputTexFormat, outputTexView, Variant.From(outputImage.GetData()).AsGodotArray<byte[]>());

        // Initialise uniform
        RDUniform inputTexUniform = new RDUniform
        {
            UniformType = RenderingDevice.UniformType.SamplerWithTexture,
            Binding = 0
        };
        inputTexUniform.AddId(samplerID);
        inputTexUniform.AddId(inputTexID);

        RDUniform outputTexUniform = new RDUniform
        {
            UniformType = RenderingDevice.UniformType.Image,
            Binding = 1
        };
        outputTexUniform.AddId(outputTexID);

        Rid uniformSet = renderingDevice.UniformSetCreate(new Godot.Collections.Array<RDUniform> { inputTexUniform, outputTexUniform }, shaderID, 0);

        Rid pipelineID = renderingDevice.ComputePipelineCreate(shaderID);
        long computeList = renderingDevice.ComputeListBegin();
        renderingDevice.ComputeListBindComputePipeline(computeList, pipelineID);
        renderingDevice.ComputeListBindUniformSet(computeList, uniformSet, 0);
        renderingDevice.ComputeListDispatch(computeList, 1, 1, 1);
        renderingDevice.ComputeListEnd();

        renderingDevice.Submit();
        renderingDevice.Sync();

        byte[] imageByteData = renderingDevice.TextureGetData(outputTexID, 0);
        shaderImage.SetData((int)texWidth, (int)texHeight, false, Image.Format.Rgbaf, imageByteData);
        shaderImage.SavePng("res://Textures/OutlineCompute.png");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }
}
