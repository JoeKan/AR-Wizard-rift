using UnityEngine;
using System.Collections;

namespace FAR{

    public class KeyboardEvent : InteractionEvent {
    
    	/*
    		Passes information about a KEY-BUTTON that was pressed on the keyboard
    	*/
    
    	private char m_key;
    	
    	public char key
    	{
    		get { return m_key; }
    	}
    	
    	public KeyboardEvent(){}
    	
    	public KeyboardEvent ( char key, InteractionMethod sender = null, InteractionEvent base_Event = null) : base(sender,base_Event)
    	{
    		this.m_key = key;
    	}
    
    
    }

}
