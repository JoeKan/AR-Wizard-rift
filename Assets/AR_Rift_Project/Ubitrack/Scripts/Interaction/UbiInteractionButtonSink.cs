using UnityEngine;
using System.Collections;
using FAR;
using System;
using System.Collections.Generic;

public class UbiInteractionButtonSink : UbiTrackComponent, FAR.Action {
    	
	private SimpleButtonReceiver m_button;

	public override void utInit(SimpleFacade simpleFacade)
	{
		base.utInit(simpleFacade);
		m_button = simpleFacade.getPushSourceButton(patternID);
		if (m_button == null)
		{
			throw new Exception("SimpleApplicationPushSourceButton not found for ID:" + patternID);
		}
	}
	
	public void sendButtonEvent(int eventID)
	{
		SimpleButton b = new SimpleButton();
		b.timestamp = UbiMeasurementUtils.getUbitrackTimeStamp();
		b._event = eventID;
		m_button.receiveButton(b);
	}
	
	public void doEvent(InteractionEvent evt) {
        if (evt is SingleKeyboardEvent)
		{
            SingleKeyboardEvent k_evt = (SingleKeyboardEvent)evt;
			sendButtonEvent((int)k_evt.key);

			

			
		}
		
	}

}
