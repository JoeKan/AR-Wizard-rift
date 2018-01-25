using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace FAR {

	public class ButtonSource : UbiInteractionMethod {

		private UnityButtonReceiver m_receiver;

        List<System.Type> m_SendableEvents = new List<System.Type>(new System.Type[] { typeof(SingleKeyboardEvent) });

        public override List<System.Type> SendableEvents
        {
            get
            {
                return m_SendableEvents;
            }
            set
            {
                m_SendableEvents = value;
            }
        }

		// Use this for initialization    
		public override void utInit(SimpleFacade simpleFacade)
		{
			base.utInit(simpleFacade);

			m_receiver = new UnityButtonReceiver();
			//if (!simpleFacade.setPushButtonCallback(patternID, m_receiver)) {
			//	throw new Exception("ButtonSource could not be set for patternID:" + patternID);
			//}

		}

		void Update() {
			Measurement<int> newData = m_receiver.getData();
			if(newData == null)
				return;
			SingleKeyboardEvent evt = new SingleKeyboardEvent((char)newData.data(),this, null);

			fireEvent(evt);
		}
}

}