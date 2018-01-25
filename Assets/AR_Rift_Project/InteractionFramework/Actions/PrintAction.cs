using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace FAR
{

    public class PrintAction : MonoBehaviour, Action
    {

        /*        Anmerkung:
        Dieses Skript ermöglicht einfache Kameradrehungen um das Geschehen. Eine Art Globus-Betrachtung.
        Um dies zu ermöglichen muss ein Empty Gameobject (Hier Rotation Center in der StartMethode) erstellt und
        als Mittelpunkt der Drehung genutzt werden.
        Sie können natürlich ein eigenes Objekt als Parent der Kamera nutzen und die hießige StartMethode einfach entfernen.
        (Vergessen sie nicht, dass die Kameradrehung auf Events basiert. Setzen sie die MainCamera mit diesem Skript
        als Listener der gewünschten Interaktionsmethode ein) */

        public GameObject ReturnObj()
        {
            return this.gameObject;
        }

        public void doEvent(InteractionEvent evt)
        {
            if (evt.GetType() == typeof(SinglePresenterKeyboardEvent)) 
            {
                Debug.Log(((SinglePresenterKeyboardEvent)evt).key);
            }

            if (evt.GetType() == typeof(PresenterKeyboardEvent))
            {
                Debug.Log(((PresenterKeyboardEvent)evt).key);
            }
        }
    }

}
