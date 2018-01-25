using UnityEngine;
using System.Collections;
using Enums;

namespace FAR
{

    public class GrabEvent : ObjectEvent
    {

        private Transform m_ParentTo;

        public Transform parentTo
        {
            get { return m_ParentTo; }
            set { m_ParentTo = value; }
        }

        public GrabEvent(GameObject obj, Transform parentTo, InteractionMethod sender = null, InteractionEvent base_Event = null)
            : base(obj, sender, base_Event)
        {
            m_ParentTo = parentTo;
        }
    }

}