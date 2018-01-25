using UnityEngine;
using System.Collections;


namespace FAR {

public enum ObjectState {Show, Hide }

	public class ObjectStateEvent : InteractionEvent {

	protected ObjectState m_state;

	public ObjectStateEvent(ObjectState state,IInteractionMethod sender = null, InteractionEvent baseEvent = null) : base(sender,baseEvent)
	{
		m_state = state;

	}


	public ObjectState objectState
	{
		get { return m_state; }
	}

}

}
