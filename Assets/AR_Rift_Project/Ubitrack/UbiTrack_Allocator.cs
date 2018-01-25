using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace FAR
{
    public class UbiTrack_Allocator : MonoBehaviour
    {
        //The names of UbiTrack-related Objects coming into this scene after load
        [HideInInspector]
        public List<string> receivedObjs;
        //References to the Objects we want to parent under the incoming UbiTrack Objects
        public List<GameObject> linkedObjs;

        [HideInInspector]
        public float UbiScaleFactor = 1000.0f;
        [HideInInspector]
        public bool differsFromNorm = false;
        [HideInInspector]
        public float customImagePlaneDistance = 0.0f;

        private float defaultImagePlaneDistance = 10.0f;
        private float farClippingPlane = 100.0f;
        private float nearClippingPlane = 0.01f;
        private int imageWidth = 1280;
        private int imageHeight = 960;

        // Use this for initialization
        void Start()
        {
            parentToUbitrackObjs();
            Debug.Log("UbiScale " + UbiScaleFactor + " Near " + nearClippingPlane + " Far " + farClippingPlane + " imageWidth " + imageWidth + " imageHeight " + imageHeight);
            changeARRiftSettings();
        }

        void OnDestroy() 
        {
            destroyLinkedObjects();
        }

        /// <summary>
        /// Uses the UbiTrackAllocator Settings to parent incoming UBiTrack Objects to given scene objects
        /// </summary>
        void parentToUbitrackObjs () 
        {
            Debug.Log("Start Parenting , we have "+receivedObjs.Count+" Received Objects");
            for ( int i = 0; i < receivedObjs.Count; i++) 
            {
                GameObject Ubi_Obj = GameObject.Find(receivedObjs[i]) as GameObject;

                if (Ubi_Obj != null)
                {
                    if (linkedObjs != null && linkedObjs[i] != null)
                    {
                        linkedObjs[i].transform.parent = Ubi_Obj.transform;
                        linkedObjs[i].transform.localPosition = new Vector3(0, 0, 0);
                        linkedObjs[i].transform.localRotation = new Quaternion(0, 0, 0, 0);
                    }
                }
                else 
                {
                    Debug.LogWarning("Couldn't find supposed incoming UbiTrack-Object " + receivedObjs[i]);
                }
            }
        }

        /// <summary>
        /// Destroys all objects linked to a received UbiTrack Object
        /// </summary>
        void destroyLinkedObjects() 
        {
            for (int i = 0; i < linkedObjs.Count; i++) 
            {
                if(linkedObjs[i] != null)
                {
                    Debug.Log("Destroying " + linkedObjs[i].name);
                    Destroy(linkedObjs[i]);
                }
            }
        }

        void changeARRiftSettings() 
        {
            //Change ImagePlaneDistance to Viewer
            GameObject ImagePlane = GameObject.Find("ImagePlane");
            if (ImagePlane == null)
            {
                Debug.LogError("Couldn't find any GameObject called ImagePlane. Cannot change its settings");
            }
            else
            {
                Vector3 imgPlanePos = ImagePlane.transform.localPosition;
                if (differsFromNorm) 
                {
                    imgPlanePos.z = customImagePlaneDistance;
                }
                else 
                {
                    imgPlanePos.z = defaultImagePlaneDistance * UbiScaleFactor;
                }
                ImagePlane.transform.localPosition = imgPlanePos;

                Vector3 imgPlaneScale = ImagePlane.transform.localScale;
                imgPlaneScale.x *= UbiScaleFactor;
                imgPlaneScale.y *= UbiScaleFactor;
                imgPlaneScale.z *= UbiScaleFactor;
                ImagePlane.transform.localScale = imgPlaneScale;
            }


            //Change RightEye Settings
            GameObject RightEyeAnchor = GameObject.Find("RightEyeAnchor");
            if (RightEyeAnchor == null) 
            {
                Debug.LogError("Couldn't find any GameObject called RightEyeAnchor. Cannot change its settings");
            }
            else 
            {
                CameraProjectionMatrixFrom3x3Matrix cpm = RightEyeAnchor.GetComponent<CameraProjectionMatrixFrom3x3Matrix>();
                if (cpm == null)
                {
                    Debug.LogError("RightEyeAnchor-Object doesn't have the required script");
                }
                else 
                {
                    cpm.nearClipping = nearClippingPlane * UbiScaleFactor;
                    cpm.farClipping = farClippingPlane * UbiScaleFactor;
                    cpm.applyProjectionMatrix();
                }
            }

            //Change LeftEye Settings
            GameObject LeftEyeAnchor = GameObject.Find("LeftEyeAnchor");
            if (LeftEyeAnchor == null)
            {
                Debug.LogError("Couldn't find any GameObject called LeftEyeAnchor. Cannot change its settings");
            }
            else
            {
                CameraProjectionMatrixFrom3x3Matrix cpm = LeftEyeAnchor.GetComponent<CameraProjectionMatrixFrom3x3Matrix>();
                if (cpm == null)
                {
                    Debug.LogError("LeftEyeAnchor-Object doesn't have the required script");
                }
                else
                {
                    cpm.nearClipping = nearClippingPlane * UbiScaleFactor;
                    cpm.farClipping = farClippingPlane * UbiScaleFactor;
                    cpm.applyProjectionMatrix();
                }
            }



        }
    }
}
