// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Custom/DynamicSurfaceKinect2Shader" {
	Properties {		
		txColor ("ColorImage", 2D) = "white" { }	    
		txDepth ("DepthImage", 2D) = "white" { }	    
		txDepthUV ("DepthImageUV", 2D) = "white" { }	
        zScale("zScale", Float) = 1
        
	}
	SubShader {
		Tags { "RenderType"="Opaque" "ARType"="DynamicSurface" }//
		LOD 200
		
		Pass
        {        
        //Cull Off
		CGPROGRAM
		#pragma only_renderers d3d11
		#pragma target 4.0
 		   
		#pragma vertex VS
		#pragma geometry GS
		#pragma fragment PS
		
		#include "ARShader.cginc"

		
	    sampler2D txColor;		
	    float4 txColor_ST;

		Texture2D<float> txDepth;		
	    float4 txDepth_ST;
	    
		Texture2D<float2> txDepthUV;		
	    float4 txDepthUV_ST;
	    
		float zScale;
		float4 xyzOffset;
	
	

	
		static const int DepthWidth = 128;
		static const int DepthHeight = 106;
		static const float2 DepthWidthHeight = float2(DepthWidth*4, DepthHeight*4);
		static const float2 DepthHalfWidthHeight = DepthWidthHeight / 2.0;
		static const float2 DepthHalfWidthHeightOffset = DepthHalfWidthHeight - 0.5;

		// vertex offsets for building a quad from a depth pixel
		static const float quadSize = 1;
		static const float2 quadOffsets[4] = 
		{
		
//    float2(0,   0  ),
//    float2(1.0, 0  ),
//    float2(0,   1.0),
//    float2(1.0, 1.0)

			 float2(quadSize,  quadSize  ),
		   float2(quadSize,   -quadSize  ),
		   
		  float2(-quadSize,   quadSize  ),
		  float2(-quadSize,   -quadSize  )
		   	    
		};

		// texture lookup offsets for sampling current and nearby depth pixels
		static const int3 texOffsets4Samples[4] =
		{
		    int3(0, 0, 0),
		    int3(1, 0, 0),
		    int3(0, 1, 0),
		    int3(1, 1, 0)
		};

		//--------------------------------------------------------------------------------------
		// Structures
		//--------------------------------------------------------------------------------------
		struct GS_INPUT
		{
			float4 Pos : SV_POSITION;
		    float4 Col : TEXCOORD0;
		};

		struct PS_INPUT
		{		    
		    float4 Pos : SV_POSITION;		
			
			float4 uv : TEXCOORD1;
		};
		
		struct vIn // Into the vertex shader
		{
			float4 vertex : POSITION;	
			float4 Col : TEXCOORD0;		
		};

	
		//--------------------------------------------------------------------------------------
		// Vertex Shader
		//--------------------------------------------------------------------------------------
		//	GS_INPUT VS( vIn v )
		//{
		//    GS_INPUT output;
		//    output.Pos= v.vertex;
		//    
		//   output.Col =  v.Col;
		// 
		//    return output;
		//}
		
		GS_INPUT VS( )		
		{
		   GS_INPUT output = (GS_INPUT)0;
		 
		    return output;
		}

		//--------------------------------------------------------------------------------------
		// Geometry Shader
		// 
		// Takes in a single vertex point.  Expands it into the 4 vertices of a quad.
		// Depth is sampled from a texture passed in of the Kinect's depth output.
		//--------------------------------------------------------------------------------------
		[maxvertexcount(16)]
		void GS(point GS_INPUT particles[1], uint primID : SV_PrimitiveID, inout TriangleStream<PS_INPUT> triStream)
		{
		    PS_INPUT output;

		
		    // texture load location for the pixel we're on 
		    int3 baseLookupCoords = int3(primID % DepthWidth, primID / DepthWidth, 0) * 4;
		    
		  
			for (int subquad = 0; subquad< 4; ++subquad){
				int3 localBaseLookupCoords = baseLookupCoords + texOffsets4Samples[subquad]*2;
			    
	
			     float4 realDepths;
			    //[unroll]
			    for (int c = 0; c < 4; ++c){
			    	float2 localOffset = quadOffsets[c];
			    	
					float3 localuv = localBaseLookupCoords;
					localuv.x +=  localOffset.x;
					localuv.y +=  localOffset.y;


					float3 uv;
					uv.xy = TRANSFORM_TEX (localuv, txDepth);  		    				    	
			    	uv.z = 0;

			    	realDepths[c] = txDepth.Load(uv);					
					
			    }
			    	    
	
	
			  
			
			    float4 avgDepth = (realDepths[0] + realDepths[1] + realDepths[2] + realDepths[3]) / 4.0; 
			    
			    // test the difference between each of the depth values and the average
			    // if any deviate by the cutoff or more, don't generate this quad
			    static const float joinCutoff = 30.0;
			    float4 depthDev = abs(realDepths - avgDepth);
			    float4 branch = step(joinCutoff, depthDev);

			    if ( any(branch) )
			    {
			        continue;
			    }

				float3 rangeCheck;
				rangeCheck.xy = TRANSFORM_TEX (localBaseLookupCoords, txDepthUV);  		    				    	
				rangeCheck.z = 0;
			
				float2 rangeUV = txDepthUV.Load(rangeCheck);

				if(rangeUV.x < 0 || rangeUV.x > 1920  || rangeUV.y < 0 || rangeUV.y > 1080 )
				{
			        continue;
			    }

			    
				// convert realDepths to meters
			    realDepths /= 1000.0;
			
			    // set the w coordinate here so we don't have to do it per vertex
			    float4 WorldPos = float4(0.0, 0.0, 0.0, 1.0);
			    
			    // Likely will be unrolled, but force anyway
			    [unroll]
			    for (int c = 0; c < 4; ++c)
			    {      
			        // convert x and y to meters
			        float localdepth = realDepths[c];//* ( -571.4011151 / xyzOffset.x);//* xyzOffset.z;			        
			        WorldPos.xy = ((localBaseLookupCoords + quadOffsets[c]) + xyzOffset.zw)  * localdepth/ xyzOffset.xy;			        
			        WorldPos.z = localdepth;
			        
			        WorldPos = mul(unity_ObjectToWorld, WorldPos);
			   
			        output.Pos = mul(UNITY_MATRIX_VP, WorldPos);			            			      			        			        		
				    
					float2 localOffset = quadOffsets[c];
			    	
					float3 uv4uvMap;
					uv4uvMap.xy = TRANSFORM_TEX (localBaseLookupCoords + localOffset, txDepthUV);  		    				    	
					uv4uvMap.z = 0;
					//uv4uvMap.w = 0;
			    	

			    	

					float2 tmpuv = txDepthUV.Load(uv4uvMap);
					output.uv.x =	tmpuv.x / 1920.0;
					output.uv.y =	tmpuv.y / 1080.0;
					output.uv.z = 0;
					output.uv.w = 0;
					//output.uv = ComputeScreenPos(output.Pos);
			        // add this vertex to the triangle strip
			        triStream.Append(output);
			    }

			    triStream.RestartStrip();
		    }
		    
		}

		//--------------------------------------------------------------------------------------
		// Pixel Shader
		//--------------------------------------------------------------------------------------
		float4 PS(PS_INPUT  i) : SV_Target
		{
		  
			
			float4 uv = i.uv;
			float2 uv_image = TRANSFORM_TEX (uv.xy, txColor);  		
			float4 color = tex2D (txColor, uv_image);
	       

	      
           return color;
		   
			
		}
		ENDCG
		}
	} 
	FallBack "Diffuse"
}
