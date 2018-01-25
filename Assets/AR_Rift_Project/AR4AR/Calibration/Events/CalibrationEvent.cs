using UnityEngine;
using System.Collections;
using FAR;

public enum CalibrationEventType {Start, Stop, Restart }

public class CalibrationEvent : InteractionEvent {

	private CalibrationEventType m_calEventType;
	
	public CalibrationEvent(CalibrationEventType calibrationType, ulong ts,IInteractionMethod sender, InteractionEvent baseEvent) : base(sender,baseEvent)
	{
		m_calEventType = calibrationType;
		
		
		
		
	}
	
	public CalibrationEventType calibrationType
	{
		get { return m_calEventType; }
	}
}
