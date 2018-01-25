using UnityEngine;
using System.Collections;

public class SPAAM_Calib_Service : MonoBehaviour {

    public string ServerURL = "http://localhost:10000";
    public string UTQLPATH = @"D:\Projekte\ARVIDA\VW_SPAAM\NewSpaam\OSTCalibrationService.dfg";
    private string utql;
    private HTTPClientHelper httpClient;
    private string ServiceInstanceURI;

    protected bool m_isStartingJob = false;
    public bool ServiceRunning = false;
    public bool CalibrateRightEye = false;

    public GetCameraIntrinsics LeftEyeIntrinsics;
    public GetCameraIntrinsics RightEyeIntrinsics;
    public GetPose LeftEyePose;
    public GetPose RightEyePose;
   
    public GetPose TargetPose;
   

	// Use this for initialization
	void Start () {
        utql = System.IO.File.ReadAllText(UTQLPATH);
        LeftEyeIntrinsics.enabled = false;
        RightEyeIntrinsics.enabled = false;

  
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown("space"))
        {
            startCalibService();
        }

        if (httpClient != null)
        {
            if (httpClient.Update())
            {


                Debug.Log("Status: " + httpClient.Status);
                if (m_isStartingJob)
                {
                    ServiceInstanceURI = httpClient.getResponseHeader()["Location"];
                    Debug.Log("response: " + ServiceInstanceURI);
                    ServiceRunning = true;
                    if(CalibrateRightEye) {
                        
                        RightEyeIntrinsics.URL = ServerURL + ServiceInstanceURI + "/sr/Intrinsics";
                        RightEyePose.URL = ServerURL + ServiceInstanceURI + "/sr/Extrinsics";

                        RightEyeIntrinsics.enabled = true;
                        RightEyePose.enabled = true;

                    } else {
                        
                        LeftEyeIntrinsics.URL = ServerURL + ServiceInstanceURI + "/sr/Intrinsics";
                        LeftEyePose.URL = ServerURL + ServiceInstanceURI + "/sr/Extrinsics";

                        LeftEyeIntrinsics.enabled = true;
                        LeftEyePose.enabled = true;
                    }
                  
                    TargetPose.URL = ServerURL + ServiceInstanceURI + "/sr/TargetPose";
                    

                }
                else
                {
                    ServiceRunning = false;
                    
                }

                httpClient = null;
            }
        }
       
	}

    public void startLeftEyeCalibration()
    {
        if (ServiceRunning)
            stopCalibServce();

        CalibrateRightEye = false;
        startCalibService();
            
    }

    public void startCalibService()
    {
        httpClient = new HTTPClientHelper(ServerURL + "/ubitrack/xmlSheet", "PUT", "text/xml", utql);
        httpClient.Start();
    }

    public void stopCalibServce()
    {
        RightEyeIntrinsics.enabled = false;
        LeftEyeIntrinsics.enabled = false;
        LeftEyePose.enabled = false;
        RightEyePose.enabled = false;
        httpClient = new HTTPClientHelper(ServerURL + ServiceInstanceURI, "DELETE");
        httpClient.Start();
    }
}
