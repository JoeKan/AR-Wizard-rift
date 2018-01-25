using UnityEngine;
using System.Collections;

namespace FAR{

    public class InteractionEvent {
    
    	/*
    		The basic InteractionEvent-Class. Contains a TIMESTAMP in Unix-ULong-Format (since 1.1.1970) 
    		and a reference to the SENDING InteractionMethod
    	*/
    
    	private ulong m_timestamp;
    	
    	private IInteractionMethod m_sender;
    	
    	/*
    		The BaseEvent is the InteractionEvent that is responsible for triggering this one.
    		Naturally it is null for StandardInteractionMethods
    		(Example: Pressing a button triggers a KeyboardEvent, it has no baseEvent)
    		Complex InteractionMethods can trigger Events WITH baseEvents
    		(The MouseObjectSelector checks for Selections of Objects. A MouseEvent ("Click") triggers
    		the Raycasting for Objects and sends a SelectionEvent. The MouseEvent is the baseEvent
    		of the SelectionEvent)
    	*/
    	private InteractionEvent m_baseEvent;
    	
    	public ulong timestamp
    	{
    		get { return m_timestamp; }
    	}
    	
    	public IInteractionMethod sender
    	{
    		get { return m_sender; }
    	}
    	
    	public InteractionEvent baseEvent
    	{
    		get { return m_baseEvent; }
    	}
    	
    	public InteractionEvent(){}
    	
    	public InteractionEvent(IInteractionMethod sender,InteractionEvent baseEvent)
    	{
            m_timestamp = UbiMeasurementUtils.getUbitrackTimeStamp();
    		m_sender = sender;
    		m_baseEvent = baseEvent;
    		
    		
    	}
    	
    	
    
    }


}