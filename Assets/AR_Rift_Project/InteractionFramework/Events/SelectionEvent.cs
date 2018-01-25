using UnityEngine;
using System.Collections;
using Enums;

namespace FAR{

    public class SelectionEvent : ObjectEvent {
    
    	/*
    		This event is sent for every "one-click" selection.
    		It differs from DragEvents that require a held-MouseButton
    	*/
    	
    
    	public SelectionEvent(GameObject obj , InteractionMethod sender = null, InteractionEvent base_Event = null) : base(obj,sender,base_Event)
    	{
    		//SelectionEvent hat noch keine eigenen Variablen die nötig sind
    	}
    }

}
