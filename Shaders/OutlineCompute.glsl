#[compute]
#version 450

// Invocations in the (x, y, z) dimension
layout(local_size_x = 8, local_size_y = 8, local_size_z = 1) in;

layout(set = 0, binding = 0) uniform sampler2D imageIn;
layout(set = 0, binding = 1, rgba32f) uniform image2D imageOut;

void main() {
	//vec2 UV = vec2(gl_GlobalInvocationID.x / textureSize(imageIn, 0).x, gl_GlobalInvocationID.y / textureSize(imageIn, 0).y);
	vec4 inPixelColour = texture(imageIn, vec2(0.5, 0.5));

	vec4 outPixelColour = inPixelColour;
	//vec4 outPixelColour = vec4(gl_GlobalInvocationID.x / 8.0, gl_GlobalInvocationID.y / 8.0, 0, 0);
	
	imageStore(imageOut, ivec2(gl_GlobalInvocationID.xy), outPixelColour);
}