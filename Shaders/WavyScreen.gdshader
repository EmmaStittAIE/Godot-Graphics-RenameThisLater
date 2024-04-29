shader_type canvas_item;

uniform sampler2D screenTexture : hint_screen_texture, repeat_disable, filter_nearest;

global uniform float waveFadeIn;

void fragment() {
	float scrollSpeed = 0.6;
	float numberOfWaves = 20.0;
	
	vec2 newUV = SCREEN_UV;
	newUV.x += sin(newUV.y * numberOfWaves - (TIME * scrollSpeed) * numberOfWaves - (numberOfWaves / 2.0)) * (waveFadeIn / 10.0);
	
	COLOR = texture(screenTexture, newUV);
}
