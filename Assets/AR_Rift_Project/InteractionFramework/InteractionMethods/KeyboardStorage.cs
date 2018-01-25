using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace FAR
{

    public class KeyboardStorage : InteractionMethod, Action
    {
        //ComplexActionInteractionMethod

        private string zeichenfolge = "";

        public GameObject ReturnObj()
        {
            return this.gameObject;
        }

        override public void Start()
        {
            base.Start();
        }

        public void doEvent(InteractionEvent evt)
        {
            if (evt.GetType() == typeof(SingleKeyboardEvent))
            {
                SingleKeyboardEvent k_evt = (SingleKeyboardEvent)evt;
                if (k_evt.key == '#')
                {
                    Debug.Log("You entered : " + zeichenfolge + " TS: " + k_evt.timestamp);

                    SendStringEvent ssevt = new SendStringEvent(zeichenfolge,this.gameObject.GetComponent<InteractionMethod>(), k_evt);
                    fireEvent(ssevt);

                    zeichenfolge = "";
                }

                else
                {
                    zeichenfolge += ((SingleKeyboardEvent)evt).key;
                }


            }

        }


        override public void Update()
        {
            base.Update();
        }

    }

}