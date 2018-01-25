using UnityEngine;
using System.Collections;
using FAR;
using System.Collections.Generic;

public class CalibrationController : InteractionMethod, Action {

    List<System.Type> m_ReceivableEvents = new List<System.Type>(new System.Type[] { typeof(SingleKeyboardEvent), typeof(HoverEvent), typeof(SelectionEvent) });

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
		if(evt is SingleKeyboardEvent) {
			SingleKeyboardEvent kevt = (SingleKeyboardEvent) evt;
			
			switch(kevt.key) {
			case 'c':
				startCalibration();
				break;
			
			case 's':
				stopCalibration();
				break;

			case 'j':
				showErrorVisualizations();
				break;
			case 'h':
				hideErrorVisualizations();
				break;
			}
		} else if(evt is HoverEvent) {
			HoverEvent hevt = (HoverEvent) evt;
			Debug.Log("HoverEvent: " + hevt.Obj.name);
		} else if(evt is SelectionEvent) {
			SelectionEvent sevt = (SelectionEvent) evt;
			Debug.Log("SelectionEvent: " + sevt.Obj.name);

			
		}


			




	}

	public void startCalibration() {

		fireEvent(new CalibrationEvent(CalibrationEventType.Start, UbiMeasurementUtils.getUbitrackTimeStamp(), this, null));
	}

	public void stopCalibration() {
		fireEvent(new CalibrationEvent(CalibrationEventType.Stop, UbiMeasurementUtils.getUbitrackTimeStamp(), this, null));
	}

	public void restartCalibration() {
		fireEvent(new CalibrationEvent(CalibrationEventType.Restart, UbiMeasurementUtils.getUbitrackTimeStamp(), this, null));
	}

	public void showErrorVisualizations() {
		fireEvent(new ObjectStateEvent(ObjectState.Show));
	}

	public void hideErrorVisualizations() {
		fireEvent(new ObjectStateEvent(ObjectState.Hide));
	}
}
