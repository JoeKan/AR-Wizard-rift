using UnityEngine;
using System.Collections;

namespace FAR{

    public class SingleKeyboardEvent : InteractionEvent {
        
        /*
            Passes information about a KEY-BUTTON that was pressed on the keyboard
        */
        
        private char m_key;
        
        public char key
        {
            get { return m_key; }
        }
        
        public SingleKeyboardEvent(){}
        
        public SingleKeyboardEvent ( char key, IInteractionMethod sender = null, InteractionEvent base_Event = null) : base(sender,base_Event)
        {
            this.m_key = key;
        }
        
        
    }

}
