// Based on: https://www.ronja-tutorials.com/post/029-tiling-noise/
const float pi = 3.14159265359;

float Rand2dTo1d(float2 value, float2 dotDir = float2(12.9898, 78.233)){
	float2 smallValue = cos(value);
	float random = dot(smallValue, dotDir);
	random = frac(sin(random) * 143758.5453);
	return random;
}

float2 Rand2dTo2d(float2 value){
	return float2(
		Rand2dTo1d(value, float2(12.9898, 78.233)),
		Rand2dTo1d(value, float2(39.346, 11.135))
	);
}

float EaseIn(float interpolator){
	return interpolator * interpolator;
}

float EaseOut(float interpolator){
	return 1 - EaseIn(1 - interpolator);
}

float EaseInOut(float interpolator){
	float EaseInValue = EaseIn(interpolator);
	float EaseOutValue = EaseOut(interpolator);
	return lerp(EaseInValue, EaseOutValue, interpolator);
}

float2 Modulo(float2 divident, float2 divisor){
	float2 positiveDivident = divident % divisor + divisor;
	return positiveDivident % divisor;
}

void TileablePerlinNoise_float(float2 value, float2 period, out float noise){
	float2 cellsMinimum = floor(value);
	float2 cellsMaximum = ceil(value);

	cellsMinimum = Modulo(cellsMinimum, period);
	cellsMaximum = Modulo(cellsMaximum, period);

	// Generate random directions
	float2 lowerLeftDirection = Rand2dTo2d(float2(cellsMinimum.x, cellsMinimum.y)) * 2 - 1;
	float2 lowerRightDirection = Rand2dTo2d(float2(cellsMaximum.x, cellsMinimum.y)) * 2 - 1;
	float2 upperLeftDirection = Rand2dTo2d(float2(cellsMinimum.x, cellsMaximum.y)) * 2 - 1;
	float2 upperRightDirection = Rand2dTo2d(float2(cellsMaximum.x, cellsMaximum.y)) * 2 - 1;

	float2 fraction = frac(value);

	// Get values of cells based on fraction and cell directions
	float lowerLeftFunctionValue = dot(lowerLeftDirection, fraction - float2(0, 0));
	float lowerRightFunctionValue = dot(lowerRightDirection, fraction - float2(1, 0));
	float upperLeftFunctionValue = dot(upperLeftDirection, fraction - float2(0, 1));
	float upperRightFunctionValue = dot(upperRightDirection, fraction - float2(1, 1));

	float interpolatorX = EaseInOut(fraction.x);
	float interpolatorY = EaseInOut(fraction.y);

	// Interpolate between values
	float lowerCells = lerp(lowerLeftFunctionValue, lowerRightFunctionValue, interpolatorX);
	float upperCells = lerp(upperLeftFunctionValue, upperRightFunctionValue, interpolatorX);

	noise = lerp(lowerCells, upperCells, interpolatorY);
	noise += 0.5;
}

void TileablePerlinNoiseDirection_float(float2 value, float2 period, out float3 noiseDirection){
	float2 cellsMinimum = floor(value);
	float2 cellsMaximum = ceil(value);

	cellsMinimum = Modulo(cellsMinimum, period);
	cellsMaximum = Modulo(cellsMaximum, period);

	// Generate random directions
	float2 lowerLeftDirection = Rand2dTo2d(float2(cellsMinimum.x, cellsMinimum.y)) * 2 - 1;
	float2 lowerRightDirection = Rand2dTo2d(float2(cellsMaximum.x, cellsMinimum.y)) * 2 - 1;
	float2 upperLeftDirection = Rand2dTo2d(float2(cellsMinimum.x, cellsMaximum.y)) * 2 - 1;
	float2 upperRightDirection = Rand2dTo2d(float2(cellsMaximum.x, cellsMaximum.y)) * 2 - 1;

	float2 fraction = frac(value);

	// Get values of cells based on fraction and cell directions
	float lowerLeftFunctionValue = dot(lowerLeftDirection, fraction - float2(0, 0));
	float lowerRightFunctionValue = dot(lowerRightDirection, fraction - float2(1, 0));
	float upperLeftFunctionValue = dot(upperLeftDirection, fraction - float2(0, 1));
	float upperRightFunctionValue = dot(upperRightDirection, fraction - float2(1, 1));

	float interpolatorX = EaseInOut(fraction.x);
	float interpolatorY = EaseInOut(fraction.y);

	// Interpolate between values
	float lowerCells = lerp(lowerLeftFunctionValue, lowerRightFunctionValue, interpolatorX);
	float upperCells = lerp(upperLeftFunctionValue, upperRightFunctionValue, interpolatorX);

	float noise = lerp(lowerCells, upperCells, interpolatorY);
	noise += 0.5;

	// Convert noise to direction
	//noise *= 2 * pi;
	//noiseDirection = float3(cos(noise), sin(noise), cos(noise));
	//noiseDirection = float3((cos(noise) + 1) / 2, (sin(noise) + 1) / 2, (cos(noise) + 1) / 2); // want -1 --> 0, 0 --> 0.5, 1 --> 1
	noiseDirection = normalize(float3(0, 1, 1));

	// So... it should be that grey (0.5, 0.5, 0.5) is neutral
}