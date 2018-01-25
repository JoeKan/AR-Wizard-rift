using UnityEngine;
using System.Collections;
using FAR;

public class CameraARShaderCustomizer : MonoBehaviour {
	    
    public Material[] stereoVideoMaterial;
	public Camera webCam_0;
	public ImageTextureUpdate webCam_0_Image;
	public Camera webCam_1;
	public ImageTextureUpdate webCam_1_Image;

	public bool preferCam0 = true;


    void OnPreRender()
    {    
	
		Camera webCam_Pref;
		Camera webCam_Other;
		Texture webCam_Pref_Image;
		Texture webCam_Other_Image;



		
		if(preferCam0) {
			webCam_Pref = webCam_0;
			webCam_Pref_Image = webCam_0_Image.getImageTexture();

			webCam_Other = webCam_1;
			webCam_Other_Image = webCam_1_Image.getImageTexture();
		} else {
			webCam_Pref = webCam_1;
			webCam_Pref_Image = webCam_1_Image.getImageTexture();
			
			webCam_Other = webCam_0;
			webCam_Other_Image = webCam_0_Image.getImageTexture();
		}

		Matrix4x4 V = webCam_Pref.transform.worldToLocalMatrix;
		Matrix4x4 P = webCam_Pref.GetComponent<CameraProjectionMatrixFrom3x3Matrix>().projectionMatrix;
		for(int i=0;i<4;i++)
			V[2,i] = -V[2,i];

	
		Matrix4x4 V_other = webCam_Other.transform.worldToLocalMatrix;
		Matrix4x4 P_other = webCam_Other.projectionMatrix;
		for(int i=0;i<4;i++)
			V_other[2,i] = -V_other[2,i];

		for(int i=0;i<stereoVideoMaterial.Length;i++){
			Material currentMat = stereoVideoMaterial[i];
			currentMat.SetMatrix("_WebPV_Pref", P * V );		
			currentMat.SetTexture("_WebCam_Pref_Image", webCam_Pref_Image);
			currentMat.SetTexture("_WebCam_Pref_Depth", webCam_Pref.targetTexture);

			
			currentMat.SetMatrix("_WebPV_Other", P_other * V_other );
			currentMat.SetTexture("_WebCam_Other_Image", webCam_Other_Image);
			currentMat.SetTexture("_WebCam_Other_Depth", webCam_Other.targetTexture);
		}
    }
	/*
	void OnPostRender() {
		// Set your materials
		
		GL.PushMatrix();
		GL.LoadProjectionMatrix(Matrix4x4.Perspective(135,
		// yourMaterial.SetPass( );
		// Draw your stuff
		GL.PopMatrix();
	}*/

}
