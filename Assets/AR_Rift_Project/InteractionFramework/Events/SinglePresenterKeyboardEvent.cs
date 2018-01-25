using UnityEngine;
using System.Collections;

namespace FAR{

    public class SinglePresenterKeyboardEvent : InteractionEvent {
        
        /*
            Passes information about a KEY-BUTTON that was pressed on the keyboard
        */
        
        private KeyCode m_key;

        public KeyCode key
        {
            get { return m_key; }
        }
        
		public SinglePresenterKeyboardEvent ( KeyCode key,IInteractionMethod sender = null, InteractionEvent base_Event = null) : base(sender,base_Event)
        {
            this.m_key = key;
        }
        
        
    }

}
