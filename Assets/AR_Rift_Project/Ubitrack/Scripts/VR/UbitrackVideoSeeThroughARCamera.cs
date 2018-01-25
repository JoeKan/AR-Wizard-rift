using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FAR;

[RequireComponent(typeof(Camera))]
public class UbitrackVideoSeeThroughARCamera : MonoBehaviour {

    public CameraProjectionMatrixFrom3x3Matrix LeftEye;
    public CameraProjectionMatrixFrom3x3Matrix RightEye;

    public Texture LeftCameraImage;
    public Texture RightCameraImage;
    public Material ARMaterial;
    public string ARMaterialTexName = "_MainTex";

    protected Camera cam;

    // Use this for initialization
    void Start () {
        cam = GetComponent<Camera>();
    }
	
	// Update is called once per frame
	void Update () {
        //if(Input.GetKeyDown(KeyCode.Space))
        updateVRSettings();

    }

    public void updateVRSettings()
    {


        
    }

    //void OnPreRender()
    void OnRenderObject()
    {
        Camera.MonoOrStereoscopicEye currentEye = cam.stereoActiveEye;

        Matrix4x4 camPose = cam.worldToCameraMatrix;
        

        Debug.Log(currentEye);
        switch (currentEye)
        {
            case Camera.MonoOrStereoscopicEye.Left:
            case Camera.MonoOrStereoscopicEye.Mono:                
                ARMaterial.SetTexture(ARMaterialTexName, LeftCameraImage);

              //  cam.SetStereoProjectionMatrix(Camera.StereoscopicEye.Left, LeftEye.projectionMatrix);

                Matrix4x4 viewLeft = new Matrix4x4();
                viewLeft.SetTRS(LeftEye.transform.localPosition, LeftEye.transform.localRotation, Vector3.one);
               // cam.SetStereoViewMatrix(Camera.StereoscopicEye.Left, viewLeft * camPose);

                break;
            case Camera.MonoOrStereoscopicEye.Right:
                ARMaterial.SetTexture(ARMaterialTexName, RightCameraImage);

               // cam.SetStereoProjectionMatrix(Camera.StereoscopicEye.Right, RightEye.projectionMatrix);

                Matrix4x4 viewRight = new Matrix4x4();
                viewRight.SetTRS(RightEye.transform.localPosition, RightEye.transform.localRotation, Vector3.one);
               // cam.SetStereoViewMatrix(Camera.StereoscopicEye.Right, viewRight * camPose);
                break;
        }


    }

    void OnPostRender()
    {
        cam.ResetStereoViewMatrices();
        //cam.ResetStereoProjectionMatrices();
    }


}
