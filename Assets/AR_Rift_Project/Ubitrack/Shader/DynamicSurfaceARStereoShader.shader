// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Custom/DynamicSurfaceARStereoShader" {
	Properties {
		txDepth ("Base (RGB)", 2D) = "white" {}
		_WebCam_Pref_Image ("WebCamPrefImage", 2D) = "white" { }
	    _WebCam_Pref_Depth ("WebCamPrefDepth", 2D) = "white" {}
	    _WebCam_Other_Image ("WebCamOtherImage", 2D) = "white" { }	    	     
	    _WebCam_Other_Depth ("WebCamOtherDepth", 2D) = "white" { }
	    _NotVisibleColor("NotVisibleColor", Color) = (0, 0, 0, 1)	    
	    _VisibleFromOtherCamColor("VisibleFromOtherCamColor", Color) = (0, 0, 1, 1)
        _NotVisibleThreshold("NotVisibleThreshold", Float) = 0.005
        zScale("zScale", Float) = 1
        
        _DistanceThresholdCameraSwitch("DistanceThresholdCameraSwitch", Float) = 0.01 
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
 		   
	
		//#pragma target 5.0
		#pragma vertex VS
		#pragma geometry GS
		#pragma fragment PS
		
		#include "ARShader.cginc"
        

		Texture2D<int> txDepth : register(t0);
		float4 txDepth_ST;
		
	    sampler2D _WebCam_Pref_Image;
		sampler2D _WebCam_Pref_Depth;
		sampler2D _WebCam_Other_Image;
		sampler2D _WebCam_Other_Depth;

	   
	    float4 _WebCam_Pref_Image_ST;
	    float4 _WebCam_Pref_Depth_ST;
	    float4 _WebCam_Other_Image_ST;
	    float4 _WebCam_Other_Depth_ST;
	    
	    float4 _VisibleFromOtherCamColor;

	     float4 _NotVisibleColor;
	     float _NotVisibleThreshold;
	     float _DistanceThresholdCameraSwitch;
	     float _PreferPrefCamera;
	    
		float4x4 _WebPV_Other;
		float4x4 _WebPV_Pref; 		
		float4x4 _WebV_Pref; 
		float4x4 _WebP_Pref; 
		
		  

		
		 matrix  CustomModelMatrix;
		 float4x4 PInv;
		 float4  XYScale;
		 float zScale;
		float4 xyzOffset;
	
	

	
		static const int DepthWidth = 160;
		static const int DepthHeight = 120;
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
			    	float2 localOffset = quadOffsets[c];
			    	localOffset.y = -localOffset.y;
			    	uv.xy = TRANSFORM_TEX (localBaseLookupCoords + localOffset, txDepth);  		    	
			    	
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

			    // set the w coordinate here so we don't have to do it per vertex
			    float4 WorldPos = float4(0.0, 0.0, 0.0, 1.0);
			    
			    // Likely will be unrolled, but force anyway
			    [unroll]
			    for (int c = 0; c < 4; ++c)
			    {      
			        // convert x and y to meters
			        float localdepth = realDepths[c] * zScale;//* ( -571.4011151 / xyzOffset.x);//* xyzOffset.z;			        
			        WorldPos.xy = ((localBaseLookupCoords + quadOffsets[c]) + xyzOffset.zw)  * localdepth/ xyzOffset.xy;			        
			        WorldPos.z = localdepth;
			        
			        
			        //WorldPos.xy = (localBaseLookupCoords + quadOffsets[c]);			        
			       	//WorldPos.z = localdepth;			       	
			       	//WorldPos = mul(PInv, WorldPos);
			        
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
		    float4 color_pref;
	        float4 color_other;
	        float4 pref_depth = getColorAndDistanceFromWorldPointAndCamera( _WebPV_Pref , i.position_in_world_space, _WebCam_Pref_Image, _WebCam_Pref_Image_ST,  _WebCam_Pref_Depth, _WebCam_Pref_Depth_ST, color_pref);
	        float4 other_depth = getColorAndDistanceFromWorldPointAndCamera( _WebPV_Other , i.position_in_world_space, _WebCam_Other_Image, _WebCam_Other_Image_ST,  _WebCam_Other_Depth, _WebCam_Other_Depth_ST, color_other);
	        
	       

	        //If the two are similar, then there is an object intersecting with our object
	                float diffPref = abs(pref_depth.z);
                 float diffOther = abs(other_depth.z);
                 
                 
                              
 			             
                 float otherBetter = diffPref - diffOther;
                 
                 
 				float4 finalColor = _NotVisibleColor;

            
 				
 				if(diffPref < _NotVisibleThreshold) {
                    	finalColor = color_pref;
                } else {
                	if(otherBetter > _DistanceThresholdCameraSwitch) {
                		if(diffOther < _NotVisibleThreshold) {
	                    	finalColor = (color_other + _VisibleFromOtherCamColor)/2; 
	                    } 
	                    //else {
                		//	finalColor = (color_pref + _NotGoodButBetterWisibleFromPrefCamColor)/2;
                		//}
                	} 
                
                }
                return finalColor;
		}
		ENDCG
		}
	} 
	FallBack "Diffuse"
}

