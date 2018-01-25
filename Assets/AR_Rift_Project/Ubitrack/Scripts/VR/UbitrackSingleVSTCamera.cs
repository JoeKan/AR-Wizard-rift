using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FAR;

public class UbitrackSingleVSTCamera : MonoBehaviour {
    
	public bool DisableOpenVRTracking = true;

    public CameraProjectionMatrixFrom3x3Matrix Eye;
    public Transform eyeOffset;

    public Texture CameraImage = null;
	public ImageTextureUpdate CameraTexture;
    
    public Material ARMaterial;
    public string ARMaterialTexName = "_MainTex";

    protected Camera cam;

    // Use this for initialization
    void Start()
    {
        cam = GetComponent<Camera>();

		//UnityEngine.VR.InputTracking.disablePositionalTracking = true;
		if (UnityEngine.XR.InputTracking.disablePositionalTracking != DisableOpenVRTracking) {
			//UnityEngine.VR.InputTracking.disablePositionalTracking = DisableOpenVRTracking;
		}
    }

  
    void OnPreRender()
    //void OnRenderObject()
	//void LateUpdate()
    {

		if (!DisableOpenVRTracking)
			return;

        StereoTargetEyeMask currentEye = cam.stereoTargetEye;



        Matrix4x4 viewOffset = new Matrix4x4();
		viewOffset.SetTRS(Vector3.zero, Quaternion.identity, new Vector3(1, 1, -1));

		if (DisableOpenVRTracking) {
			transform.localPosition = Vector3.zero;
			transform.localRotation = Quaternion.identity;
		}


		Matrix4x4 viewMatrix = new Matrix4x4 ();
		viewMatrix = viewOffset * eyeOffset.worldToLocalMatrix;


		cam.projectionMatrix = Eye.projectionMatrix;

		transform.localPosition = eyeOffset.transform.localPosition;
		transform.localRotation = eyeOffset.transform.localRotation;

		if (CameraImage == null)
			CameraImage = CameraTexture.getImageTexture ();

    	ARMaterial.SetTexture(ARMaterialTexName, CameraImage);

        switch (currentEye)
        {
            case StereoTargetEyeMask.None:
            case StereoTargetEyeMask.Both:
            case StereoTargetEyeMask.Left:
                
			//cam.SetStereoProjectionMatrix(Camera.StereoscopicEye.Left, newProjectionMatrix);
			//cam.SetStereoViewMatrix(Camera.StereoscopicEye.Left, viewOffset);  

                //Matrix4x4 viewLeft = new Matrix4x4();
                //viewLeft.SetTRS(Eye.transform.localPosition, Eye.transform.localRotation, Vector3.one);
			cam.SetStereoViewMatrix(Camera.StereoscopicEye.Left, viewMatrix);
                break;
            case StereoTargetEyeMask.Right:
			//cam.SetStereoProjectionMatrix(Camera.StereoscopicEye.Right, newProjectionMatrix);
			cam.SetStereoViewMatrix(Camera.StereoscopicEye.Right, viewMatrix);  
                break;
            
        }
   
    }

    void OnPostRender()
    {
        //cam.ResetStereoViewMatrices();
        //cam.ResetStereoProjectionMatrices();
    }

}
