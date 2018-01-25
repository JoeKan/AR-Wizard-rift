using UnityEngine;
using System.Collections;

public class ShowARTCalibrationPanel : MonoBehaviour {

	public GameObject ARTCalibGUI;

	void Update () {
		if (Input.GetKeyDown (KeyCode.U)) {
			ARTCalibGUI.SetActive(!ARTCalibGUI.activeInHierarchy);
		}
	}
}
