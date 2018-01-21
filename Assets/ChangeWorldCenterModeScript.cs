using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class ChangeWorldCenterModeScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        VuforiaARController.Instance.SetWorldCenterMode(VuforiaARController.WorldCenterMode.FIRST_TARGET);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
