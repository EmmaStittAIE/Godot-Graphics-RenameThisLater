shader_type canvas_item;

uniform sampler2D screenTexture : hint_screen_texture, repeat_disable, filter_nearest;

global uniform bool waveEffect;

void fragment() {
	if (waveEffect)
	{
		float scrollSpeed = 0.4;
		float numberOfWaves = 20.0;
	
		vec2 newUV = SCREEN_UV;
		newUV.x += sin(newUV.y * numberOfWaves - (TIME * scrollSpeed) * numberOfWaves - (numberOfWaves / 2.0)) / 10.0;

		COLOR = texture(screenTexture, newUV);
	}
}
