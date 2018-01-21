using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChangeWandSparks : MonoBehaviour {

    ParticleSystem system;
    Color initialColor;

	// Use this for initialization
	void Start () {
        system = GetComponent<ParticleSystem>();
        var mainTmp = system.main;
        mainTmp.startColor = Color.blue;
	}
	
	// Update is called once per frame
	void Update () {
        var mainTmp = system.main;
        if (Input.GetMouseButton(0))
            mainTmp.startColor = Color.yellow;
        else
            mainTmp.startColor = Color.blue;
    }
}
