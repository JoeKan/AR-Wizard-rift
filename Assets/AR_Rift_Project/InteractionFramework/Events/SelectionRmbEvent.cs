using UnityEngine;
using System.Collections;
using Enums;

namespace FAR
{
    public class SelectionRmbEvent : ObjectEvent
    {

        /*
    		This event is sent for every "one-click" selection.
    		It differs from DragEvents that require a held-MouseButton
    	*/

        public SelectionRmbEvent(GameObject obj, Vector3 hit, IInteractionMethod sender = null, InteractionEvent base_Event = null)
            : base(obj, hit,sender, base_Event)
        {
            //SelectionEvent hat noch keine eigenen Variablen die nötig sind
        }
    }

}
