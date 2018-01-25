using UnityEngine;
using System.Collections;
using FAR;

public class GetCameraIntrinsics : MonoBehaviour {
    private HTTPClientHelper httpClient;
    public string URL = "http://localhost:10000/ubitrack/instances/0/sr/Intrinsic";
    public int standardWidth = 640;
    public int standardHeight = 480;
    public float nearClipping = 0.01f;
    public float farClipping = 100.0f;
    public Camera Camera;
    
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (httpClient != null)
        {
            if (httpClient.IsDone)
            {
                string response = httpClient.GetResponseBody();
                //Debug.Log(response);

                ARVIDAGraph graph = new ARVIDAGraph(URL);
                graph.readRDFText(response, URL);
                RDFTerm term = graph.getUriTerm(new System.Uri(URL));

                FAR.Measurement<float[]> intrinsics = RDF2Ubitrack.getMatrix3x3(term);

                Matrix4x4 projectionMatrix = CameraUtils.constructProjectionMatrix3x3(intrinsics.data(), standardWidth, standardHeight, nearClipping, farClipping);
//                Debug.Log(projectionMatrix);
                Camera.projectionMatrix = projectionMatrix;

                httpClient = null;
            }
        }
        else
        {
            httpClient = new HTTPClientHelper(URL, "GET");
            httpClient.Start();
        }
	}
}
