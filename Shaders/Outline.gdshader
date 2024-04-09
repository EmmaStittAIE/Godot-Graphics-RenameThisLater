shader_type spatial;

uniform sampler2D outlineSilhouetteTex;

void vertex(){
	POSITION = vec4(VERTEX, 1.0);
}

void fragment() {
	vec2 sizeOfPixel = 1.0 / vec2(textureSize(outlineSilhouetteTex, 0));
	
	vec2 thisPixelSample = texture(outlineSilhouetteTex, vec2(SCREEN_UV)).xy;
	
	vec2 neighbourSamples[8];
	neighbourSamples[0] = texture(outlineSilhouetteTex, vec2(SCREEN_UV.x - sizeOfPixel.x, SCREEN_UV.y - sizeOfPixel.y)).xy;
	neighbourSamples[1] = texture(outlineSilhouetteTex, vec2(SCREEN_UV.x - sizeOfPixel.x, SCREEN_UV.y)).xy;
	neighbourSamples[2] = texture(outlineSilhouetteTex, vec2(SCREEN_UV.x - sizeOfPixel.x, SCREEN_UV.y + sizeOfPixel.y)).xy;
	neighbourSamples[3] = texture(outlineSilhouetteTex, vec2(SCREEN_UV.x, SCREEN_UV.y - sizeOfPixel.y)).xy;
	neighbourSamples[4] = texture(outlineSilhouetteTex, vec2(SCREEN_UV.x, SCREEN_UV.y + sizeOfPixel.y)).xy;
	neighbourSamples[5] = texture(outlineSilhouetteTex, vec2(SCREEN_UV.x + sizeOfPixel.x, SCREEN_UV.y - sizeOfPixel.y)).xy;
	neighbourSamples[6] = texture(outlineSilhouetteTex, vec2(SCREEN_UV.x + sizeOfPixel.x, SCREEN_UV.y)).xy;
	neighbourSamples[7] = texture(outlineSilhouetteTex, vec2(SCREEN_UV.x + sizeOfPixel.x, SCREEN_UV.y + sizeOfPixel.y)).xy;
	
	for (int i = 0; i < 8; i++) {
		// If the sample is undefined, we want to skip over this
		if (neighbourSamples[i] != vec2(0, 0))
		{
			// If this pixel is undefined, we colour it the sample's colour
			if (thisPixelSample == vec2(0, 0)) {
				ALBEDO = vec3(neighbourSamples[i], 0);
			}
			else {
				float distToThisSeed = length(thisPixelSample - SCREEN_UV);
				float distToSampleSeed = length(neighbourSamples[i] - SCREEN_UV);
				
				if (distToThisSeed > distToSampleSeed) {
					ALBEDO = vec3(neighbourSamples[i], 0);
				}
			}
		}
	}
}
