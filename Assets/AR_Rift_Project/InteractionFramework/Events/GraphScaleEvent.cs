using UnityEngine;
using System.Collections;
using FAR;

public class GraphScaleEvent : InteractionEvent {

	string m_GrowShrink;
	GameObject m_selectedOne;

	public string GrowShrink
	{
		get { return m_GrowShrink; }
	}

	public GameObject SelectedOne
	{
		get { return m_selectedOne; }
	}

    public GraphScaleEvent(GameObject selectedOne, string GrowShrink, InteractionMethod sender = null, InteractionEvent base_Evt = null)
        : base(sender, base_Evt)
	{
		m_GrowShrink = GrowShrink;
		m_selectedOne = selectedOne;
	}
}
