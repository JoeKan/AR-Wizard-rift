using UnityEngine;
using System.Collections;

namespace FAR{

    public class HoverEvent : ObjectEvent {

        public HoverEvent(GameObject obj, Vector3 hit,IInteractionMethod sender = null, InteractionEvent base_Event = null)
            : base(obj, hit, sender, base_Event)
        {
            //HoverEvent hat noch keine eigenen Variablen die nötig sind
        }
    }

}
