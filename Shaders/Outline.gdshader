shader_type spatial;

uniform sampler2D outlineSilhouetteTex;

void vertex(){
	POSITION = vec4(VERTEX, 1.0);
}

void fragment() {
	float silhouettePixel = texture(outlineSilhouetteTex, SCREEN_UV).x;
	
	vec2 positionSamples[9];
	
	positionSamples[0] = texture(outlineSilhouetteTex, vec2(SCREEN_UV.x - 1.0, SCREEN_UV.y - 1.0)).xy;
	positionSamples[1] = texture(outlineSilhouetteTex, vec2(SCREEN_UV.x, SCREEN_UV.y - 1.0)).xy;
	positionSamples[2] = texture(outlineSilhouetteTex, vec2(SCREEN_UV.x + 1.0, SCREEN_UV.y - 1.0)).xy;
	positionSamples[3] = texture(outlineSilhouetteTex, vec2(SCREEN_UV.x -1.0, SCREEN_UV.y)).xy;
	positionSamples[4] = texture(outlineSilhouetteTex, vec2(SCREEN_UV.x, SCREEN_UV.y)).xy;
	positionSamples[5] = texture(outlineSilhouetteTex, vec2(SCREEN_UV.x + 1.0, SCREEN_UV.y)).xy;
	positionSamples[6] = texture(outlineSilhouetteTex, vec2(SCREEN_UV.x - 1.0, SCREEN_UV.y + 1.0)).xy;
	positionSamples[7] = texture(outlineSilhouetteTex, vec2(SCREEN_UV.x, SCREEN_UV.y + 1.0)).xy;
	positionSamples[8] = texture(outlineSilhouetteTex, vec2(SCREEN_UV.x + 1.0, SCREEN_UV.y + 1.0)).xy;
	
	float distBetweenSamples[9];
	
	for (int i = 0; i < 9; i++) {
		if (positionSamples[i] != vec2(0, 0)) {
			distBetweenSamples[i] = length(positionSamples[i] - SCREEN_UV);
		}
	}
	
	if (positionSamples[4].x > 0.0)
	{
		ALBEDO = vec3(SCREEN_UV, 0);
	}
	
	//ALBEDO = vec3(silhouettePixel);
}
