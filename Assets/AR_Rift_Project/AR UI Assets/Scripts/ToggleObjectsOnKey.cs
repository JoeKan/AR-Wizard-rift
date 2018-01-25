using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleObjectsOnKey : MonoBehaviour {

	public KeyCode key;

	public List<GameObject> objects = new List<GameObject>();

	void Update () {
		if (Input.GetKeyDown (key)) {
			foreach (GameObject go in objects) {
				go.SetActive(!go.activeSelf);
			}
		}
	}
}
