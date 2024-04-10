using Godot;
using System;

public partial class ExampleCompute : Node
{
	public override void _Ready()
	{
		RenderingDevice renderingDevice = RenderingServer.CreateLocalRenderingDevice();

		// Load shader
		RDShaderFile shaderFile = GD.Load<RDShaderFile>("res://Shaders/ExampleCompute.glsl");
		RDShaderSpirV shaderSpirV = shaderFile.GetSpirV();
		Rid shaderID = renderingDevice.ShaderCreateFromSpirV(shaderSpirV);

		// Prepare data
		float[] inputFloats = new float[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
		byte[] inputBytes = new byte[inputFloats.Length * sizeof(float)];
		Buffer.BlockCopy(inputFloats, 0, inputBytes, 0, inputBytes.Length);

		// Create storage buffer
		Rid bufferID = renderingDevice.StorageBufferCreate((uint)inputBytes.Length, inputBytes);

		// Initialise uniform
		RDUniform uniform = new RDUniform
		{
			UniformType = RenderingDevice.UniformType.StorageBuffer,
			Binding = 0
		};
		uniform.AddId(bufferID);
		Rid uniformSet = renderingDevice.UniformSetCreate(new Godot.Collections.Array<RDUniform> { uniform }, shaderID, 0);

		Rid pipelineID = renderingDevice.ComputePipelineCreate(shaderID);
		long computeList = renderingDevice.ComputeListBegin();
		renderingDevice.ComputeListBindComputePipeline(computeList, pipelineID);
		renderingDevice.ComputeListBindUniformSet(computeList, uniformSet, 0);
		renderingDevice.ComputeListDispatch(computeList, 5, 1, 1);
		renderingDevice.ComputeListEnd();

		renderingDevice.Submit();
		renderingDevice.Sync();

		byte[] outputBytes = renderingDevice.BufferGetData(bufferID);
		float[] outputFloats = new float[inputFloats.Length];
		Buffer.BlockCopy(outputBytes, 0, outputFloats, 0, outputBytes.Length);

		GD.Print("Input: ", string.Join(", ", inputFloats));
		GD.Print("Output: ", string.Join(", ", outputFloats));
	}

	public override void _Process(double delta)
	{
	}
}
