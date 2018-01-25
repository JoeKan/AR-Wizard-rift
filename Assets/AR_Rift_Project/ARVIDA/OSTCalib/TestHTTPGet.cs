using UnityEngine;
using System.Collections;

public class TestHTTPGet : MonoBehaviour {

    private HTTPClientHelper httpClient;
    public string URL = "http://localhost:10000/ubitrack/instances/0/sr/TestSensorOutput";
    public FAR.UbitrackRelativeToUnity Relative = FAR.UbitrackRelativeToUnity.Local;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown("space"))
        {
           
        }

        if (httpClient != null)
        {
            if (httpClient.IsDone)
            {
                string response = httpClient.GetResponseBody();
                Debug.Log(response);

                ARVIDAGraph graph = new ARVIDAGraph(URL);
                graph.readRDFText(response, URL);
                RDFTerm term = graph.getUriTerm(new System.Uri(URL));

                FAR.Measurement<FAR.Pose> pose = RDF2Ubitrack.getPose(term);

                FAR.UbiUnityUtils.setGameObjectPose(Relative, this.gameObject, pose.data());


                httpClient = null;
            }
        } else {
             httpClient = new HTTPClientHelper(URL, "GET");
            httpClient.Start();
        }
	}
}
