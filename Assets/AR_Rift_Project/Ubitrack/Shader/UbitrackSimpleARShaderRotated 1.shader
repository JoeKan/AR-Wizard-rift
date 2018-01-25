Shader "Custom/UbitrackSimpleARShaderRotated" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}		 
	}
	SubShader {
		Tags { "RenderType"="Opaque" }		
		
		LOD 200
		
		Lighting Off
		CGPROGRAM
		//#pragma surface surf NoLighting
		#pragma surface surf NoLighting noforwardadd

		sampler2D _MainTex;		
		float4 _MainTex_ST;
		
		struct Input {			
			float4 screenPos;
		};

		fixed4 LightingNoLighting(SurfaceOutput s, fixed3 lightDir, fixed atten)
		{
			fixed4 c;
			c.rgb = s.Albedo;
			c.a = s.Alpha;
			return c;
		}

		void surf (Input IN, inout SurfaceOutput o) {
			float2 screenUV = (IN.screenPos.xy / IN.screenPos.w);			
			
			float2 screenUV2;
			screenUV2.x = screenUV.y;
			screenUV2.y = screenUV.x;
			
			float2 uv_image = TRANSFORM_TEX (screenUV2, _MainTex);  		
			
			half4 c = tex2D (_MainTex, uv_image);
			o.Albedo = c.rgb;
			
			o.Alpha = 1.0f;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
