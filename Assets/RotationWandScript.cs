using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationWandScript : MonoBehaviour {

    public GameObject sphereToTurnTo;

	// Use this for initialization
	void Start () {
        this.transform.rotation = Quaternion.LookRotation(sphereToTurnTo.transform.position);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
