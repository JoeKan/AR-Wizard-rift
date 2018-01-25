using UnityEngine;
using System.Collections;
using Enums;

namespace FAR{

    public class ObjectEvent : InteractionEvent {
    
    	/*
    		StandardEvent for any ObjectInteraction.
    		That is any interaction that requires selection of an object
    	*/
    
    	private GameObject m_Obj;
        private Vector3 m_hitPoint;
    	
    	public GameObject Obj
    	{
    		get { return m_Obj; }
    	}

        public Vector3 Hit
        {
            get { return m_hitPoint; }
        }
    

		public ObjectEvent (GameObject obj , Vector3 hit,IInteractionMethod sender = null, InteractionEvent base_Evt = null) : base(sender,base_Evt)
    	{
    		this.m_Obj = obj;
            this.m_hitPoint = hit;

    	}

        public ObjectEvent(GameObject obj,IInteractionMethod sender, InteractionEvent base_Evt)
            : base(sender, base_Evt)
        {
            this.m_Obj = obj;
        }


    }

}
