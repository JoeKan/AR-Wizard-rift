using UnityEngine;
using System.Collections;
using FAR;

public class CameraDefaultARShaderCustomizer : MonoBehaviour {
	public Material[] stereoVideoMaterial;
	public ImageTextureUpdate webCam_0_Image;
	public ImageTextureUpdate webCam_1_Image;
	
	public bool preferCam0 = true;
	
	
	void OnPreRender()
	{    

		Texture webCam_Pref_Image;
//		Texture webCam_Other_Image;
		
		
		
		
		if(preferCam0) {
			webCam_Pref_Image = webCam_0_Image.getImageTexture();
			
//			webCam_Other_Image = webCam_1_Image.getImageTexture();
		} else {
			webCam_Pref_Image = webCam_1_Image.getImageTexture();
			
//			webCam_Other_Image = webCam_0_Image.getImageTexture();
		}
		

		for(int i=0;i<stereoVideoMaterial.Length;i++){
			Material currentMat = stereoVideoMaterial[i];
			currentMat.SetTexture("_MainTex", webCam_Pref_Image);
		}
	}
}
