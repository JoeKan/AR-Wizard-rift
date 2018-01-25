// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Custom/StaticSurfaceARStereoShader" {
    Properties
	   {	   	    
	    _WebCam_Pref_Image ("WebCamPrefImage", 2D) = "white" { }
	    _WebCam_Pref_Depth ("WebCamPrefDepth", 2D) = "white" {}
	    _WebCam_Other_Image ("WebCamOtherImage", 2D) = "white" { }	    	     
	    _WebCam_Other_Depth ("WebCamOtherDepth", 2D) = "white" { }
	    _NotVisibleColor("NotVisibleColor", Color) = (0, 0, 0, 1)
	    _VisibleFromOtherCamColor("VisibleFromOtherCamColor", Color) = (0, 0, 1, 1)
	    _NotGoodButBetterWisibleFromPrefCamColor("NotGoodButBetterWisibleFromPrefCamColor", Color) = (0, 1, 0, 1)
        _NotVisibleThreshold("NotVisibleThreshold", Float) = 0.005
        
        _DistanceThresholdCameraSwitch("DistanceThresholdCameraSwitch", Float) = 0.01       
         
	   }
   
   
    SubShader
    {
        //Tags {  "Queue" = "Transparent" "RenderType"="Transparent"  } "ARType"="ARStaticSurface" 
        Tags { "RenderType"="Opaque" "ARType"="StaticSurface" }
 
        Pass
        {
        	
            CGPROGRAM
            #pragma target 3.0
            #pragma vertex vert
            #pragma fragment frag
            #include "ARShader.cginc"
           
            sampler2D _WebCam_Pref_Image;	
 			sampler2D _WebCam_Pref_Depth;	
 			sampler2D _WebCam_Other_Image; 			
 			sampler2D _WebCam_Other_Depth;	
 			
 			   
            float4 _WebCam_Pref_Image_ST;
            float4 _WebCam_Pref_Depth_ST;
            float4 _WebCam_Other_Image_ST;
            float4 _WebCam_Other_Depth_ST;
            
            float4 _VisibleFromOtherCamColor;
            float4 _NotGoodButBetterWisibleFromPrefCamColor;
 
 			
            uniform float4 _NotVisibleColor;
            uniform float _NotVisibleThreshold;
            uniform float _DistanceThresholdCameraSwitch;
            uniform float _PreferPrefCamera;
            
			float4x4 _WebPV_Other;
			float4x4 _WebPV_Pref; 					
 			 
            struct v2f
            {
              	float4 pos : SV_POSITION;
				float4 uv : TEXCOORD0;				
				float4 position_in_world_space : TEXCOORD1;
            };
         
            v2f vert(appdata_base v)
            {
               	v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = ComputeScreenPos (o.pos);				
				o.position_in_world_space = 
               		mul(unity_ObjectToWorld, v.vertex);				
				return o;
            }
 
            float4 frag(v2f i) : COLOR
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
        Fallback "VertexLit"
    }
