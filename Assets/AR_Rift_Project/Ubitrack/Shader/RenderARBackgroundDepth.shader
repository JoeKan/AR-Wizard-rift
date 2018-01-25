// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

Shader "Custom/RenderARBackgroundDepth" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
	}
	  SubShader
    {
        //Tags {  "Queue" = "Transparent" "RenderType"="Transparent"  }
        Tags { "RenderType"="Opaque" }
 
        Pass
        {
        	Blend SrcAlpha OneMinusSrcAlpha
            //ZWrite Off
            //Cull Off
 
            CGPROGRAM
// Upgrade NOTE: excluded shader from DX11 and Xbox360; has structs without semantics (struct v2f members position_in_world_space)
#pragma exclude_renderers d3d11 xbox360
            #pragma target 3.0
            #pragma vertex vert
            #pragma fragment frag
            #pragma glsl
            #include "UnityCG.cginc"
 
            sampler2D _MainTex;	
 			
                        
			float4x4 _M;
			float4x4 _V;
			float4x4 _P;
			 
            struct v2f
            {
              	float4 pos : SV_POSITION;
				float4 uv : TEXCOORD0;				
				float4 position_in_world_space;
				float3 normal : TEXCOORD3; //you don't need these semantics except for XBox360
				float3 viewT : TEXCOORD2; //you don't need these semantics except for XBox360
            };
            
            float4 _MainTex_ST;
            
            v2f vert(appdata_base v)
            {
               	v2f o;
				
				float2 uv = v.texcoord.xy / v.texcoord.w; 
				//uv.x = uv.x * 256.0/640;           	
				//uv.y = uv.y * 256.0/480;
 				uv = TRANSFORM_TEX (uv, _MainTex); 
				//float4 depth =  tex2Dlod(_MainTex, uv);
				float4 depth =float4 (1,1,1,1);
				float z_world = depth.w / 1000;
				float x_world = (640*uv.x - 310.49887084960937) * z_world / 529.52777099609375;
				float y_world = (480*uv.y - 217.56852722167969) * z_world / 529.01959228515625;
				
				float4 newPos;
				newPos = mul(unity_ObjectToWorld, v.vertex);
				
				newPos.x = newPos.x + depth.y;
				newPos.y = newPos.y + depth.z;
				newPos.z = newPos.z + depth.x;				
				
				newPos.w = 1;
				
				
				
				v.vertex = mul(unity_WorldToObject, newPos);
				//v.vertex.x = x_world;
				//v.vertex.y = y_world;
				//v.vertex.y = v.vertex.y - depth.y;
				
				
				
				o.pos = UnityObjectToClipPos(v.vertex);
				
				o.uv = UnityObjectToClipPos(v.vertex);	
	
				o.position_in_world_space = depth; 
               		//mul(_Object2World, v.vertex);

				o.normal = normalize(v.normal);
				o.viewT = normalize(ObjSpaceViewDir(v.vertex));//ObjSpaceViewDir is similar, but localspace.//WorldSpaceViewDir

				return o;
            }
 
            float4 frag(v2f i) : COLOR
            {
            	clip(i.position_in_world_space.z - 0.010);
            	
            	float angle = dot (i.normal, i.viewT);
            	//clip(angle - 0.5);
				
            	float2 uv = i.uv.xy / i.uv.w;            	
 				uv = TRANSFORM_TEX (uv, _MainTex); 
 				
                float4 c = tex2D (_MainTex, uv);;                
                c.x = i.position_in_world_space.w;
                c.y = 0;
                c.z = 1;
                c.w = 1;
 
 
                return c;
            }
 
            ENDCG
        }
    }
        Fallback "Transparent/VertexLit"
    }




