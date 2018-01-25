// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Custom/RenderDynamicDepth" {
	Properties {
		txDepth ("Base (RGB)", 2D) = "white" {}
		
	}
	SubShader {
		Tags { "RenderType"="Opaque" }//"ARType"="ARDynamicSurface"
		LOD 200
		
		Pass
        {        
        //Cull Off
		CGPROGRAM
		#pragma only_renderers d3d11
		#pragma target 4.0
 		   
	
		//#pragma target 5.0
		#pragma vertex VS
		#pragma geometry GS
		#pragma fragment PS
		
		#include "ARShader.cginc"
        

		Texture2D<int> txDepth : register(t0);
		float4 txDepth_ST;

		
		 matrix  CustomModelMatrix;
		 float4  XYScale;
		
	
		//--------------------------------------------------------------------------------------
		// Constants
		//--------------------------------------------------------------------------------------
		static const float4 playerColorCoefficients[8] = 
		{
		    float4(1.0,  1.0,  1.0,  1.0), 
		    float4(1.4,  0.8,  0.8,  1.0),
		    float4(0.8,  1.4,  0.8,  1.0),
		    float4(0.8,  0.8,  1.4,  1.0),
		    float4(0.6,  1.2,  1.2,  1.0),
		    float4(1.2,  0.6,  1.2,  1.0),
		    float4(1.2,  1.2,  0.6,  1.0),
		    float4(1.3,  0.9,  0.8,  1.0)
		};


	
		static const int DepthWidth = 160;
		static const int DepthHeight = 120;
		static const float2 DepthWidthHeight = float2(DepthWidth, DepthHeight);
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
			float4 position_in_world_space : TEXCOORD0;
			float4 uv : TEXCOORD1;
		};
		
		struct vIn // Into the vertex shader
		{
			float4 vertex : POSITION;	
			float4 Col : TEXCOORD0;		
		};

		GS_INPUT VS( vIn v )
		{
		    GS_INPUT output;
		    output.Pos= v.vertex;
		    
		    output.Col =  v.Col;
		 
		    return output;
		}
		//--------------------------------------------------------------------------------------
		// Vertex Shader
		//--------------------------------------------------------------------------------------
		//GS_INPUT VS( )
		//{
		 //   GS_INPUT output = (GS_INPUT)0;
		 
		//    return output;
		//}

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

		    // use the minimum of near mode and standard
		    static const int minDepth = 300 << 16;

		    // use the maximum of near mode and standard
		    static const int maxDepth = 4000 << 16;

		    // texture load location for the pixel we're on 
		    int3 baseLookupCoords = int3(primID % DepthWidth, primID / DepthWidth, 0) * 4;
		    
		  
			for (int subquad = 0; subquad< 4; ++subquad){
				int3 localBaseLookupCoords = baseLookupCoords + texOffsets4Samples[subquad]*2;
			    int4 depths;
				
			     
			    [unroll]
			    for (int c = 0; c < 4; ++c){
			    	float3 uv;
			    	//uv.xy = baseLookupCoords + texOffsets4Samples[c];
			    	uv.xy = TRANSFORM_TEX (localBaseLookupCoords + texOffsets4Samples[c], txDepth);  		    	
			    	
			    	uv.z = 0;
			    	depths[c] = txDepth.Load(uv);		        		    
			    }
			    	    
			    // remove player index information
			   float4 realDepths = depths / 65536.0;

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

			    // constant for all c
			    int player = depths[0] & 0x7;

			    float4 baseColor = float4(1,1,1,1);//playerColorCoefficients[player];

			  // convert realDepths to meters
			    realDepths /= 1000.0;

			    // log scale to push brighter
			    // normalize z to between 0 and 1, where 0 is min depth and 1 is max depth
			    // log2 of player depth max-min is precomputed and passed into the z component of playerDepthMinMax
			    // flip by subtracting it from 1.0 to make closer brighter
			    float4 zNormalized = 1 - (realDepths-0.35) / 4.0 ;

			  
			    // set the w coordinate here so we don't have to do it per vertex
			    float4 WorldPos = float4(0.0, 0.0, 0.0, 1.0);
			    
			    // Likely will be unrolled, but force anyway
			    [unroll]
			    for (int c = 0; c < 4; ++c)
			    {      
			        // convert x and y to meters
			        WorldPos.xy = ((localBaseLookupCoords + quadOffsets[c]) - DepthHalfWidthHeightOffset*4) * XYScale.xy * realDepths[c];
			        WorldPos.z = realDepths[c];
			        
			        WorldPos = mul(unity_ObjectToWorld, WorldPos);
			        //WorldPos.x = quadOffsets[c] ;
			        //WorldPos.y = 1;
			        //WorldPos.z = quadOffsets[c] ;
			             
			        //WorldPos = WorldPos * 10;
			        output.Pos = mul(UNITY_MATRIX_VP, WorldPos);			            			      			        			        		
					output.position_in_world_space = WorldPos;		
					output.uv = ComputeScreenPos(output.Pos);
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
	        return i.uv;
		}
		ENDCG
		}
	} 
	FallBack "Diffuse"
}

