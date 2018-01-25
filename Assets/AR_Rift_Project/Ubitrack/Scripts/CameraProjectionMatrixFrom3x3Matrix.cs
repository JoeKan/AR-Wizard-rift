using UnityEngine;
using System.Collections;
using System;

namespace FAR
{

    public class CameraProjectionMatrixFrom3x3Matrix : UbiTrackComponent
    {

        public int standardWidth = 640;
        public int standardHeight = 480;
        public float nearClipping = 0.01f;
        public float farClipping = 100.0f;

        private SimpleApplicationPullSinkMatrix3x3 m_matrixPull = null;

        public Matrix4x4 projectionMatrix;
        public Measurement<float[]> intrinsics = null;

        public bool Flip_X_Y = false;

        // Use this for initialization
        public override void utInit(SimpleFacade simpleFacade)
        {
            base.utInit(simpleFacade);

            m_matrixPull = simpleFacade.getPullSinkMatrix3x3(patternID);
            if (m_matrixPull == null)
            {
                throw new Exception("SimpleApplicationPushSourceButton not found for ID:" + patternID);
            }

        }

        public override void utStart()
        {
            base.utStart();
            applyProjectionMatrix();
        }

        public void applyProjectionMatrix()
        {
            Debug.Log("Calling vanilla function");
            SimpleMatrix3x3 matrix = new SimpleMatrix3x3();
            if (m_matrixPull.getMatrix3x3(matrix, UbiMeasurementUtils.getUbitrackTimeStamp()))
            {

                intrinsics = UbiMeasurementUtils.ubitrackToUnity(matrix);

                projectionMatrix = CameraUtils.constructProjectionMatrix3x3(intrinsics.data(), standardWidth, standardHeight, nearClipping, farClipping);

                if (Flip_X_Y)
                {
                    Matrix4x4 rotMat = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0, 0, -90), Vector3.one);
                    projectionMatrix = rotMat * projectionMatrix;
                }

                //Debug.Log(projectionMatrix);
                if (GetComponent<Camera>() != null)
                {
                    GetComponent<Camera>().projectionMatrix = projectionMatrix;
                    //Debug.Log("set projection matrix of camera");
                }
            }
            else
            {
                throw new Exception("unable to get 3x3 matrix");
            }
        }

        public void applyProjectionMatrix(ulong givenTimestamp)
        {
            Debug.Log("Trying to pull with timestamp " + givenTimestamp);
            SimpleMatrix3x3 matrix = new SimpleMatrix3x3();
            if (m_matrixPull.getMatrix3x3(matrix, givenTimestamp))
            {

                intrinsics = UbiMeasurementUtils.ubitrackToUnity(matrix);

                projectionMatrix = CameraUtils.constructProjectionMatrix3x3(intrinsics.data(), standardWidth, standardHeight, nearClipping, farClipping);

                if (Flip_X_Y)
                {
                    Matrix4x4 rotMat = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0, 0, -90), Vector3.one);
                    projectionMatrix = rotMat * projectionMatrix;
                }

                //Debug.Log(projectionMatrix);
                if (GetComponent<Camera>() != null)
                {
                    GetComponent<Camera>().projectionMatrix = projectionMatrix;
                    //Debug.Log("set projection matrix of camera");
                }
            }
            else
            {
                throw new Exception("unable to get 3x3 matrix");
            }
        }

        public void applyProjectionMatrix(ulong givenTimestamp, float screenWidth, float screenHeight)
        {
            Debug.Log("Trying to pull with timestamp " + givenTimestamp);
            SimpleMatrix3x3 matrix = new SimpleMatrix3x3();
            if (m_matrixPull.getMatrix3x3(matrix, givenTimestamp))
            {

                intrinsics = UbiMeasurementUtils.ubitrackToUnity(matrix);

                projectionMatrix = CameraUtils.constructProjectionMatrix3x3(intrinsics.data(), screenWidth, screenHeight, nearClipping, farClipping);

                if (Flip_X_Y)
                {
                    Matrix4x4 rotMat = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0, 0, -90), Vector3.one);
                    projectionMatrix = rotMat * projectionMatrix;
                }

                //Debug.Log(projectionMatrix);
                if (GetComponent<Camera>() != null)
                {
                    GetComponent<Camera>().projectionMatrix = projectionMatrix;
                    //Debug.Log("set projection matrix of camera");
                }
            }
            else
            {
                throw new Exception("unable to get 3x3 matrix");
            }
        }
    }
}
