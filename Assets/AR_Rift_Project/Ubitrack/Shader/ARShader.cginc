#ifndef ARSHADER_CG_INCLUDED
#define ARSHADER_CG_INCLUDED

#include "UnityCG.cginc"

inline fixed4 getColorAndDistanceFromWorldPointAndCamera( float4x4 cameraVP , float4 worldPos, sampler2D colorTexture, float4 colorTexture_ST,  sampler2D depthTexture, float4 depthTexture_ST, out float4 color)
{
	float4 depth;
	
	float4 pos = mul(cameraVP, worldPos);
	float4 rawuv = ComputeScreenPos (pos);
	depth.x = rawuv.z; // sceneZ_pref
	
	float4 uv = rawuv / rawuv.w;
	float2 uv_image = TRANSFORM_TEX (uv.xy, colorTexture);  		
	color = tex2D (colorTexture, uv_image);
	
	float2 uv_depth = TRANSFORM_TEX (uv.xy, depthTexture);  	
	float4 depthFromTexture = tex2D (depthTexture, uv_depth);	
	
	//if(abs(pos.x) > 0.5 || abs(pos.y) > 0.5 ){
	//if(uv_depth.x <0.5 || uv_depth.y  < 0)	{
	if(uv_depth.x < 0 || uv_depth.y  < 0 || uv_depth.x > 1 || uv_depth.y  > 1)	{
		depthFromTexture.z = -1000;
	}

	depth.y = depthFromTexture.z; // zCamPref
	
	depth.z = depth.x - depth.y;
	
	return depth;
}


#endif
