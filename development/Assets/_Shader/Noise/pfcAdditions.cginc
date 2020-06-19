#include "SimplexNoiseGrad3D.cginc"

float4 _NoiseParams2; // (amplitude, frequency, speed)
float3 _GlobalCurrentVectorA;
float3 _GlobalCurrentVectorB;
float3 _GlobalCurrentDirectionNormalized;
float4 _GlobalCurrentHeightSettings;
sampler3D _VelocityTex;
float4x4 _WorldToVolume;
float4x4 _VolumeToWorld;

// Divergence-free noise field - p should be in worldspace!
// returns the amount of noise to be applied, also in worldspace
float3 CurrentSimulationNoise(float3 p)
{
	float3 worldSpacePoint = p;
	
	p *= _NoiseParams2.y;
	p += float3(0.9, 1.0, 1.1) * (_NoiseParams2.z * _Time.y);
	// p.y *= 1 + _NoiseParams2.w;
	float3 n1 = snoise_grad(p);
	float3 n2 = snoise_grad(p + float3(15.3, 13.1, 17.4));
	float3 simplexNoise = cross(n1, n2) * _NoiseParams2.x;

	// add a custom animated current vector to everything (in worldspace)
	simplexNoise += 
		smoothstep(_GlobalCurrentHeightSettings.x, _GlobalCurrentHeightSettings.y, worldSpacePoint.y) * 
		lerp(_GlobalCurrentVectorA, _GlobalCurrentVectorB, 
			smoothstep(_GlobalCurrentHeightSettings.z, _GlobalCurrentHeightSettings.w, dot(_GlobalCurrentDirectionNormalized, worldSpacePoint)));

#ifndef _USEVELOCITYFIELD_OFF
	float3 uvw = mul(_WorldToVolume, float4(worldSpacePoint, 1)).xyz; // -1..1 space
	uvw = (uvw + 1) * 0.5; // 0..1 space

	float3 volumeNoise = tex3Dlod(_VelocityTex, float4(uvw, 0)) * 2 - 1;
	volumeNoise = mul(_VolumeToWorld, float4(volumeNoise, 0)) * 1;

	return simplexNoise + volumeNoise;
#else
	return simplexNoise;
#endif
}

// Divergence-free noise field - p should be in worldspace!
// returns the amount of noise to be applied, also in worldspace 
float3 CurrentSimulationNoise_Custom(float3 p, float4 noiseParams)
{
	float3 worldSpacePoint = p;

	p *= noiseParams.y;
	p += float3(0.9, 1.0, 1.1) * (noiseParams.z * _Time.y);
	// p.y *= 1 + _NoiseParams2.w;
	float3 n1 = snoise_grad(p);
	float3 n2 = snoise_grad(p + float3(15.3, 13.1, 17.4));
	float3 simplexNoise = cross(n1, n2) * noiseParams.x;

	return simplexNoise;
}

void CurrentSimulationNoise_Custom_float(float3 worldPosition, float4 noiseParams, out float3 result) {
	float3 worldSpacePoint = worldPosition;

	worldPosition *= noiseParams.y;
	worldPosition += float3(0.9, 1.0, 1.1) * (noiseParams.z * _Time.y);
	// p.y *= 1 + _NoiseParams2.w;
	float3 n1 = snoise_grad(worldPosition);
	float3 n2 = snoise_grad(worldPosition + float3(15.3, 13.1, 17.4));
	float3 simplexNoise = cross(n1, n2) * noiseParams.x;

	result = simplexNoise;
}