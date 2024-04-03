using Godot;
using System;

public partial class OutlineMaskCompute : Node
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		RenderingDevice rd = RenderingServer.CreateLocalRenderingDevice();

        RDShaderFile shaderFile = GD.Load<RDShaderFile>("res://Scripts/OutlineMaskCompute.cs");
		RDShaderSpirV shaderSpirV = shaderFile.GetSpirV();
		Rid shader = rd.ShaderCreateFromSpirV(shaderSpirV);

		float[] input = new float[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
		byte[] inputBytes = new byte[input.Length * sizeof(float)];
		Buffer.BlockCopy
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
