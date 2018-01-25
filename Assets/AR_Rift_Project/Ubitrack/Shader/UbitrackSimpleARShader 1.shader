Shader "Custom/UbitrackSimpleARShader" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}		
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		

		CGPROGRAM
		#pragma surface surf Lambert

		sampler2D _MainTex;		
		float4 _MainTex_ST;
		
		struct Input {			
			float4 screenPos;
		};

		void surf (Input IN, inout SurfaceOutput o) {
			float2 screenUV = (IN.screenPos.xy / IN.screenPos.w);			
			
			float2 uv_image = TRANSFORM_TEX (screenUV, _MainTex);  		
			
			half4 c = tex2D (_MainTex, screenUV);
			o.Albedo = c.rgb;
			
			o.Alpha = c.a;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
