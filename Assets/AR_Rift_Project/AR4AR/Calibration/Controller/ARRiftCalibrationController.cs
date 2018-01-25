using UnityEngine;
using System.Collections;
using FAR;
using UnityEngine.UI;
using System.Collections.Generic;

public class ARRiftCalibrationController : MonoBehaviour, Action  {
	public DefaultUbiCalibService[] Calibrations;

	public Text CalibrationNameField;
	public Text StatusTextField;


	public CameraDefaultARShaderCustomizer LeftCamera;
	public CameraDefaultARShaderCustomizer RightCamera;


	public DefaultAction ApplyIntrinsicsAction;

	private int CurrentCalibration = -1;

    List<System.Type> m_ReceivableEvents = new List<System.Type>(new System.Type[] { typeof(SingleKeyboardEvent) });

    public List<System.Type> ReceivableEvents
    {
        get
        {
            return m_ReceivableEvents;
        }
        set
        {
            m_ReceivableEvents = value;
        }
    }

	public void doEvent(InteractionEvent evt){

		int oldCalib = CurrentCalibration;
		bool toogleCalibState = false;



		if(evt.GetType() == typeof(SingleKeyboardEvent)) {
			SingleKeyboardEvent kevt = (SingleKeyboardEvent) evt;


			switch(kevt.key) {
			case '0':
				toogleCalibState = true;
				CurrentCalibration = 0;
				break;
			case '1':
				toogleCalibState = true;
				CurrentCalibration = 1;
				break;
			case '2':
				toogleCalibState = true;
				CurrentCalibration = 2;
				break;
			case '3':
				toogleCalibState = true;
				CurrentCalibration = 3;
				break;
			case '4':
				toogleCalibState = true;
				CurrentCalibration = 4;
				break;
			case '5':
				toogleCalibState = true;
				CurrentCalibration = 5;
				break;
			case '6':
				toogleCalibState = true;
				CurrentCalibration = 6;
				break;
			case '7':
				toogleCalibState = true;
				CurrentCalibration = 7;
				break;
			case '8':
				toogleCalibState = true;
				CurrentCalibration = 8;
				break;
			case '9':
				toogleCalibState = true;
				CurrentCalibration = 9;
				break;

			case 'q': // mono vision right cam
				LeftCamera.preferCam0 = false;
				RightCamera.preferCam0 = false;
				LeftCamera.transform.localPosition = new Vector3(0.034f,-0.065f,0f);
				LeftCamera.transform.localRotation = Quaternion.identity;

				break;
			case 'w': // mono vision left cam
				LeftCamera.preferCam0 = true;
				RightCamera.preferCam0 = true;
				LeftCamera.transform.localPosition = new Vector3(0.034f,-0.065f,0f);
				LeftCamera.transform.localRotation = Quaternion.identity;

				break;
			case 'e': // mono vision right cam
				LeftCamera.preferCam0 = true;
				RightCamera.preferCam0 = false;

				break;

			case 'a':
				ApplyIntrinsicsAction.doEvent(null);
				break;

			default:
				// do nothing
				break;
			

			}



		}

		if(toogleCalibState) {		
			if(oldCalib >= 0 && oldCalib != CurrentCalibration && Calibrations[oldCalib].ServiceRunning) {
				Calibrations[oldCalib].stopService();
			}
			
			if(CurrentCalibration >= 0) {
				toggleCalibration(CurrentCalibration);
				
				CalibrationNameField.text = Calibrations[CurrentCalibration].ServiceName;
			}		
		}
		
	
		
		
	}

	public void Update() {
		if(CurrentCalibration >= 0 && Calibrations[CurrentCalibration].ServiceRunning) {
			StatusTextField.text = Calibrations[CurrentCalibration].getStatusText();
		} else {
			StatusTextField.text = "";
		}
	}

	public void toggleCalibration(int index) {
		if(Calibrations[index].ServiceRunning) {
			Calibrations[index].doEvent(new CalibrationEvent(CalibrationEventType.Stop, UbiMeasurementUtils.getUbitrackTimeStamp(), null, null));
		} else {
			Calibrations[index].doEvent(new CalibrationEvent(CalibrationEventType.Start, UbiMeasurementUtils.getUbitrackTimeStamp(), null, null));
		}
	}
}

