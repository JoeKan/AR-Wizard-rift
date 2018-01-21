using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.WSA.Input;
using UnityEngine.SceneManagement;

public class ButtonGestureRecognitionScriptCalibrate : MonoBehaviour {

    GestureRecognizer recognizer;

	// Use this for initialization
	void Start () {
        recognizer = new GestureRecognizer();
        recognizer.SetRecognizableGestures(GestureSettings.Tap);
        recognizer.TappedEvent += MyTapEventHandler;
        recognizer.StartCapturingGestures();
    }

    void MyTapEventHandler(UnityEngine.XR.WSA.Input.InteractionSourceKind source, int tapCount, Ray headRay) {
        SceneManager.LoadScene("Calibrate");
    }

    // Update is called once per frame
    void Update () {
		
	}
}
