// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Custom/UbitrackARShaderDifferentCamera" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		 _ImageWidth ("_UbiWidthFactor", Float) = 0.11625
		 _ImageHeight ("_UbiHeightFactor", Float) = 0.119375		 		 
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		

		CGPROGRAM
		#pragma surface surf Lambert

		sampler2D _MainTex;		
		float _UbiWidthFactor;
		float _UbiHeightFactor;
		float4x4 _WebCamProjectionMatrix;
		
		float4x4 _WebM;
		float4x4 _WebV;
		float4x4 _WebP;
		float4x4 _WebPV;
		
		float4 _MainTex_ST;
		
		struct Input {			
			float4 screenPos;
			float3 worldPos;
		};

		void surf (Input IN, inout SurfaceOutput o) {
			
			float4x4 localMVP =  mul( mul(UNITY_MATRIX_P, UNITY_MATRIX_V), unity_ObjectToWorld );
			//float4x4 localMVP = mul(UNITY_MATRIX_P, UNITY_MATRIX_V);
            float4 point;
            point.xyz = IN.worldPos;
            point.w = 1;
            float4 sp = mul(localMVP, point);           	           
			
			//float2 wcoord = sp.xy;
			float2 wcoord = sp.xy/ sp.w;
			
			wcoord = wcoord/ _ScreenParams.xy;
			
			wcoord.x = (0.5f*wcoord.x) + 0.5f;
        	wcoord.y = (0.5f*wcoord.y) + 0.5f;
        	
        	//wcoord = sp.xy/ _ScreenParams.xy;
			
        	float2 uv = TRANSFORM_TEX (wcoord, _MainTex);          
			half4 c = tex2D (_MainTex, uv);
			o.Albedo = c.rgb;
			
			o.Alpha = c.a;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
