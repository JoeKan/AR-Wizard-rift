using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Enums;

namespace FAR{

    public class ButtonAction : MonoBehaviour, Action {
    
    	//Kommt an jeden Button um ihn mit MouseEvents zu verknüpfen

		public GameObject ReturnObj()
		{
			return this.gameObject;
		}
    
    	public void doEvent(InteractionEvent evt){
    		
    
    		if(evt.GetType() == typeof(SendStringEvent))
    		{
    			SendStringEvent sevt = (SendStringEvent) evt;
    			
    			if(this.gameObject.name.Contains(sevt.message))
    			{
                    //this.GetComponent<Button>().OnClicked();
    			}
    		}
    		
    		if(evt.GetType() == typeof(SelectionEvent))
    		{
    			SelectionEvent sevt = (SelectionEvent) evt;
    			
    			if(this.gameObject == sevt.Obj)
    			{
    				//this.GetComponent<Button>().OnClicked();
    			}
    		}
    
    	}
    	
    
    
    }

}
