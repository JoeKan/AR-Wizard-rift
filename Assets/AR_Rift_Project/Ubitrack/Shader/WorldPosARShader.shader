// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: commented out 'float4x4 _CameraToWorld', a built-in variable
// Upgrade NOTE: replaced '_CameraToWorld' with 'unity_CameraToWorld'

Shader "WorldPosArShader" {
Properties {
    _MainTex ("Base (RGB)", 2D) = "white" {}
}
 
SubShader {
    Pass {
 
   
CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#include "UnityCG.cginc"
 
uniform sampler2D _MainTex;
 
struct appdata {
    float4 vertex : POSITION;
    float3 texcoord : TEXCOORD0;
};
 
struct v2f {
    float4 pos : SV_POSITION;
    float4 uv : TEXCOORD0;
    float3 ray : TEXCOORD1;
};
 
 
 
v2f vert (appdata v)
{
    v2f o;
    o.pos = UnityObjectToClipPos(v.vertex);
    o.uv = ComputeScreenPos (o.pos);
	//o.ray = mul (UNITY_MATRIX_MV, v.vertex).xyz * float3(-1,-1,1);
	//Use of UNITY_MATRIX_MV is detected. To transform a vertex into view space, consider using UnityObjectToViewPos for better performance.
	//float3 UnityObjectToViewPos(float3 pos)	
	//Transforms a point from object space to view space. This is the equivalent of mul(UNITY_MATRIX_MV, float4(pos, 1.0)).xyz, and should be used in its place.
	o.ray = UnityObjectToViewPos(v.vertex) * float3(-1,-1,1);
	
    // v.texcoord is equal to 0 when we are drawing 3D light shapes and
    // contains a ray pointing from the camera to one of near plane's
    // corners in camera space when we are drawing a full screen quad.
    o.ray = lerp(o.ray, v.texcoord, v.texcoord.z != 0);
   
    return o;
}
 
sampler2D _CameraDepthTexture;
// float4x4 _CameraToWorld;
float3 _clipPoint;
float3 _clipNormal;
 
half4 frag (v2f i) : COLOR
{
    i.ray = i.ray * (_ProjectionParams.z / i.ray.z);
    float2 uv = i.uv.xy / i.uv.w;
   
    float depth = UNITY_SAMPLE_DEPTH(tex2D (_CameraDepthTexture, uv));
    depth = Linear01Depth (depth);
    float4 vpos = float4(i.ray * depth,1);
    float3 wpos = mul (unity_CameraToWorld, vpos).xyz;
   
    return half4(wpos*50,1.0f);
   
}
 
ENDCG
}
 
}
Fallback Off
}