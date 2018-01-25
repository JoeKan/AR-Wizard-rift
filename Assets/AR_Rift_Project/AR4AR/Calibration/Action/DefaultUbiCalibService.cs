using UnityEngine;
using System.Collections;
using FAR;
using System;
using UnityEngine.UI;

public class DefaultUbiCalibService : UbitrackCalibrationService {
	public string ServiceName;
	public string InstructionText;

	public UbiTrackComponent[] RequiredInput;

	public Text StatusTextField;



	//public override void ServiceStarted() {
	//	if(StatusTextField != null) StatusTextField.text = "Running";
	//	fireEvent(new ObjectStateEvent(ObjectState.Show, UbiMeasurementUtils.getUbitrackTimeStamp(), this, null));
	//}
	
	//public override void ServiceStopped() {
	//	if(StatusTextField != null)StatusTextField.text = "Standby";
	//	fireEvent(new ObjectStateEvent(ObjectState.Hide, UbiMeasurementUtils.getUbitrackTimeStamp(), this, null));
	//}

	public override void startService() {
		if(StatusTextField != null)StatusTextField.text = "Starting ...";
		base.startService();
		
		
		
	}
	
	public override void stopService() {
		if(StatusTextField != null)StatusTextField.text = "Stopping ...";
		base.stopService();
		
	}

	public string getStatusText() {
		string result = "";
		foreach(UbiTrackComponent ps in RequiredInput) {
			if(ps.isTimeout()) {
				result += "<color=red>" + ps.gameObject.name + "</color>\n";
		 	} else {
				result += "<color=green>" + ps.gameObject.name + "</color>\n";
			}
		}

		return result;
	}
}
