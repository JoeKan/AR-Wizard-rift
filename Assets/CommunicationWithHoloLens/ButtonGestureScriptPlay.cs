using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.WSA.Input;
using UnityEngine.SceneManagement;

public class ButtonGestureScriptPlay : MonoBehaviour {

    GestureRecognizer recognizer;

    // Use this for initialization
    void Start()
    {
        recognizer = new GestureRecognizer();
        recognizer.SetRecognizableGestures(GestureSettings.Tap);
        recognizer.TappedEvent += MyTapEventHandler;
        recognizer.StartCapturingGestures();
    }

    void MyTapEventHandler(UnityEngine.XR.WSA.Input.InteractionSourceKind source, int tapCount, Ray headRay)
    {
        //if we had a client running (scene Calibrate was opened first) we need to close the client before loading the next scene
        if (SceneManager.GetActiveScene().Equals("Calibrate"))
        {
            NetworkPoseSink poseSink = new NetworkPoseSink();
            poseSink.StopClient();
        }
        SceneManager.LoadScene("Play");
    }

    // Update is called once per frame
    void Update () {
		
	}
}
