shader_type spatial;

uniform sampler2D depthTexture: source_color, hint_depth_texture;

void vertex(){
	POSITION = vec4(VERTEX, 1.0);
}

void fragment() {
	float depth = texture(depthTexture, SCREEN_UV).x;
	//vec3 ndc = vec3(SCREEN_UV * 2.0 - 1.0, depth);
	
	//vec4 view = INV_PROJECTION_MATRIX * vec4(ndc, 1.0);
	//view.xyz /= view.w;
	//float linearDepth = -view.z;

	if (depth < 1.0) {
		ALBEDO = vec3(SCREEN_UV, 0);
	}
	else {
		ALBEDO = vec3(0.0);
	}
	
	//ALBEDO = vec3(linear_depth);
}