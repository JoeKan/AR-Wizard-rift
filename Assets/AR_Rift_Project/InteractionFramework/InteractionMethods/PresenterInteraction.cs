using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Enums;

namespace FAR{

    public class PresenterInteraction : InteractionMethod {
    
    
    	// Use this for initialization
    	override public void Start () {
    		
            base.Start();
    	}
    	
    	// Update is called once per frame
		override public void Update()
		{
			base.Update();

    		if(Input.anyKey)
    		{
        
    			if(Input.GetMouseButton(0))
    			{
    				MouseEvent evt = new MouseEvent(Input.mousePosition.x,Input.mousePosition.y,MouseButton.Left,MouseState.Down,this.gameObject.GetComponent<MouseInteraction>(),null);
    				fireEvent(evt);
    			}
    			
    			if(Input.GetMouseButton(1))
    			{
    				MouseEvent evt = new MouseEvent(Input.mousePosition.x,Input.mousePosition.y,MouseButton.Right,MouseState.Down,this.gameObject.GetComponent<MouseInteraction>(),null);
    				fireEvent(evt);
    			}
    			
    			if(Input.GetMouseButton(2))
    			{
    				MouseEvent evt = new MouseEvent(Input.mousePosition.x,Input.mousePosition.y,MouseButton.Wheel,MouseState.Down,this.gameObject.GetComponent<MouseInteraction>(),null);
    				fireEvent(evt);
    			}	
    			
    
    		}
    		
    		else if(Input.GetMouseButtonUp(0))
    		{
    			MouseEvent evt = new MouseEvent(Input.mousePosition.x,Input.mousePosition.y,MouseButton.Left,MouseState.Up,this.gameObject.GetComponent<MouseInteraction>(),null);
    			fireEvent(evt);
    		}
    		
    		else if(Input.GetMouseButtonUp(1))
    		{
    			MouseEvent evt = new MouseEvent(Input.mousePosition.x,Input.mousePosition.y,MouseButton.Right,MouseState.Up,this.gameObject.GetComponent<MouseInteraction>(),null);
    			fireEvent(evt);
    		}
    		
    		else if(Input.GetMouseButtonUp(2))
    		{
    			MouseEvent evt = new MouseEvent(Input.mousePosition.x,Input.mousePosition.y,MouseButton.Wheel,MouseState.Up,this.gameObject.GetComponent<MouseInteraction>(),null);
    			fireEvent(evt);
    		}
            
            else
            {
                MouseEvent evt = new MouseEvent(Input.mousePosition.x,Input.mousePosition.y,MouseButton.NONE,MouseState.NONE,this.gameObject.GetComponent<MouseInteraction>(),null);
                fireEvent(evt);           
            }
    		
    		
    	}
    	
    	
    	
    }

}
