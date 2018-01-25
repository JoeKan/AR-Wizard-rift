using UnityEngine;
using System.Collections;
using FAR;
using System;
using UnityEngine.UI;

public class TipCalibrationService : UbitrackCalibrationService {
	public Text StatusTextField;
	public Text TimeTextField;
	public Text VelocityTextField;
	public PoseSource CalibObject;
	public Material SpeedMaterial;
	public float StopCalibrationAfterXSeconds = 15f;
	public float MinVelocity = 150f;
	public float MaxVelocity = 300f;
	
	private float m_remainingTime = 0;
	


	
	public override void Update() {
		base.Update();

		if(ServiceRunning){
			StatusTextField.text = "Running";
			if(StopCalibrationAfterXSeconds > 0) {
				m_remainingTime = StopCalibrationAfterXSeconds;
			}
		}
		else 
			StatusTextField.text = "Standby";

		m_remainingTime -= Time.deltaTime;
		if(m_remainingTime > 0) {
			setTimeField(m_remainingTime);
		} else {
			//if(ServiceRunning)
			//	stopService();
			setTimeField(StopCalibrationAfterXSeconds);
		}
		
		float mm_s = CalibObject.Velocity * 1000;
		
		VelocityTextField.text = String.Format ("{0:#####} mm/s", mm_s);
		
		float alpha = 1f;
		if(mm_s < MinVelocity) {
			alpha = 0f;
		} else if(mm_s < MaxVelocity) {
			alpha = (mm_s - MinVelocity) / (MaxVelocity - MinVelocity);
		}
		
		Color speedColor = Color.Lerp(Color.green, Color.red, alpha);
		VelocityTextField.color = speedColor;
		SpeedMaterial.color = speedColor;
	}
	
	private void setTimeField(float time) {
		//int minutes = (int)time / 60;
		int seconds = (int)time % 60;
		int fraction = (int)(time * 100) % 100;
		//TimeTextField.text = String.Format ("{0:00}:{1:00}:{2:00}", minutes, seconds, fraction);
		TimeTextField.text = String.Format ("{0:00}:{1:00}", seconds, fraction);
	}
	
	public override void startService() {
		StatusTextField.text = "Starting ...";
		base.startService();
		
		
		
	}
	
	public override void stopService() {
		StatusTextField.text = "Stopping ...";
		base.stopService();
		
	}
}
