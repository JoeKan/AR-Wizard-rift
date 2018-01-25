using UnityEngine;
using System.Collections;

namespace FAR
{

    /// <summary>
    /// This <see cref="CharacterEvent"/> isssues the CharacterAssistant to rotate towards the target.
    /// </summary>
    public class CharacterRotaionEvent : InteractionEvent
    {
        private Transform m_roationTargetTransform;

        /// <summary>
        /// The target of the rotation
        /// </summary>
        public Transform RotationTargetTransform
        {
            get { return m_roationTargetTransform; }
        }

        /// <summary>
        /// Constructs a new CharacterRotationEvent
        /// </summary>
        /// <param name="rotationTargetTranform">The target the CharacterAssistant should rotate towards.</param>
        /// <param name="ts">The event's timestamp.</param>
        /// <param name="sender">The event's sender.</param>
        /// <param name="base_Evt">The event's base event.</param>
        public CharacterRotaionEvent(Transform rotationTargetTranform,InteractionMethod sender = null, InteractionEvent base_Evt = null) 
            :base(sender, base_Evt)
        {
            this.m_roationTargetTransform = rotationTargetTranform;
        }
    }
}
