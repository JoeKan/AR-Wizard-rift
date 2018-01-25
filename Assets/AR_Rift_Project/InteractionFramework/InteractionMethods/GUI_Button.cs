using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace FAR{

public class GUI_Button : InteractionMethod, Action {

	/*
		Idea:
		The GUI_Button is a complex interaction method.
		It is considered a Distribution-Point that forwards incoming Events to all its Listeners when
		its 3D-Model is clicked on.
		For that it reacts to Selection-/and Object-Events that are aimed at itself or one of its listeners.
		
		If the model is clicked on, the ObjectEvent will be sent to all its listeners.
		If an Object-/SelectionEvent comes in that is not aimed at the Button it will simply forward
		the event to all of its listeners.
		
		In addition:
		At this point also KeyboardEvents will be forwarded.
	*/
	

		public GameObject ReturnObj()
		{
			return this.gameObject;
		}


	// Use this for initialization
	override public void Start()
	{
		base.Start();
	}
	
	// Update is called once per frame
	override public void Update()
    {
        base.Update();
	
	}
	
		
	public void doEvent(InteractionEvent evt)
	{
	
		if(evt.GetType() == typeof(ObjectEvent) || evt.GetType() == typeof(SelectionEvent))
		{
		
			ObjectEvent oevt = (ObjectEvent) evt;
			//Wenn der Button ausgewählt wurde
			if(oevt.Obj == this.gameObject)
			{
				ButtonEvent bevt = new ButtonEvent(this.gameObject,this.GetComponent<InteractionMethod>(),oevt);
				fireEvent(bevt);
			}
			
			//Wenn ein Objekt ausgewählt wurde das nicht der Button ist
			else
			{
				SelectionEvent sevt = new SelectionEvent(oevt.Obj,this.GetComponent<InteractionMethod>(),evt);
				fireEvent(sevt);
			}
		}
		if(evt.GetType() == typeof(KeyboardEvent))
		{
			KeyboardEvent kevt = (KeyboardEvent) evt;
			fireEvent(kevt);
		}
	
	}
	
	

}

}