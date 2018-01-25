using UnityEngine;
using System.Collections;
using FAR;

public enum OculusGUIEvent {ResetGUI, ResetTracking }

public class OculusEvent : InteractionEvent {
	private OculusGUIEvent m_GUIEvent;

	public OculusEvent(OculusGUIEvent guiEvent,IInteractionMethod sender = null, InteractionEvent baseEvent = null) : base(sender,baseEvent)
	{
		m_GUIEvent = guiEvent;

	
		
		
	}

	public OculusGUIEvent GUIEvent
	{
		get { return m_GUIEvent; }
	}

}
