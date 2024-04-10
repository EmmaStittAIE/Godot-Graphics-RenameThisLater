shader_type spatial;
render_mode unshaded, cull_disabled;

uniform sampler2D screenTexture : hint_screen_texture, repeat_disable, filter_nearest;

global uniform bool waveEffect;

void vertex(){
	POSITION = vec4(VERTEX * float(waveEffect), 1.0);
}

void fragment() {
	if (waveEffect) {
		float scrollSpeed = 0.6;
		float numberOfWaves = 20.0;
	
		vec2 newUV = SCREEN_UV;
		newUV.x += sin(newUV.y * numberOfWaves - (TIME * scrollSpeed) * numberOfWaves - (numberOfWaves / 2.0)) / 10.0;

		ALBEDO = texture(screenTexture, newUV).xyz;
	}
}
