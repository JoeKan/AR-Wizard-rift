using UnityEngine;
using System.Collections;

namespace FAR{

public class ButtonEvent : InteractionEvent {

	private GameObject m_Button;
	
	
	
	public GameObject Button
	{
		get { return m_Button; }
	}
	
	public ButtonEvent(){}
	
	public ButtonEvent (GameObject button ,InteractionMethod sender = null, InteractionEvent base_Evt = null) : base(sender,base_Evt)
	{
		this.m_Button = button;	
	}
}

}
