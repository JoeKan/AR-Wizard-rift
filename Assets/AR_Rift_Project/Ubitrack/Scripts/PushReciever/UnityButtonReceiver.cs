using UnityEngine;
using System.Collections;
using System;

namespace FAR{

	public class UnityButtonReceiver : SimpleButtonReceiver {

		protected Measurement<int> m_data = null;

		private System.Object thisLock = new System.Object();  
		
		public Measurement<int> getData()		 
		{
			lock (thisLock)
			{
				Measurement<int> tmp = m_data;
				m_data = null;
				return tmp;		
			}
			
		}

		public override void receiveButton (SimpleButton button) {
			lock (thisLock)
			{		
				m_data = new Measurement<int>(button._event, button.timestamp);
			}
		}

	}
}