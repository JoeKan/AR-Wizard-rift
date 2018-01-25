using UnityEngine;
using System.Collections;
using System;

namespace FAR{

	public class UbiTrackComponent : MonoBehaviour
	{
	    public string patternID;
	    public UTFacade facade = null;
		
		
		protected SimpleFacade ubiFacade = null;

		//public int TimeoutInMilliSeconds = 40;

		protected bool m_timeout = true;

	
	    public void Awake(){
	        if (facade == null)
	        {
				facade = (UTFacade) GameObject.FindObjectOfType(typeof(UTFacade));			
	        }
			
			bool wasEnabled = this.enabled;
			this.enabled = false;
			
			if (facade == null)
				throw new Exception("SimpleFacade must not be null");
			
	        if(wasEnabled)
	            facade.registerUbitrackComponent(this);
	        
	    }
	
	    public virtual void utInit(SimpleFacade simpleFacade)
	    {         
	        if (patternID == null || patternID.Length == 0)
	        {
	            throw new MissingMemberException("patternID is not set in gameobject: "+gameObject.name);
	        }
			ubiFacade = simpleFacade;
	    }
	    
	    public virtual void utDestroy()
	    {
	    }
	
	    public virtual void utStart()
	    {
	        this.enabled = true;
	    }
	
	    public virtual void utStop()
	    {
	        this.enabled = false;
	    }

		public virtual void pullNow(ulong lastTimestamp) {
		}

			
		public bool isTimeout() {
			return m_timeout;
		}
	}

}
