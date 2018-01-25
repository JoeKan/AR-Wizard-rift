using UnityEngine;
using System.Collections;

namespace FAR
{
    /// <summary>
    /// This <see cref="CharacterEvent"/> issues the CharacterAssistant to move towards the passed target.
    /// </summary>
    public class CharacterNavigationEvent : CharacterEvent 
    {
        private Vector3 m_navigationTargetPosition;

        /// <summary>
        /// This property returns the target point the CharacterAssistant should move to.
        /// </summary>
        public Vector3 NavigationTargetPosition
        {
            get { return m_navigationTargetPosition; }
        }

        /// <summary>
        /// This Constructor creates a new CharacterNavigationEvent.
        /// </summary>
        /// <param name="navigationTargetPosition">The point the CharacterAssistant should move to.</param>
        /// <param name="ts">The event's timestamp.</param>
        /// <param name="sender">The event's sender.</param>
        /// <param name="base_Evt">The event's base event.</param>
        public CharacterNavigationEvent(Vector3 navigationTargetPosition, InteractionMethod sender = null, InteractionEvent base_Evt = null)
            : base(sender, base_Evt)
        {
            this.m_navigationTargetPosition = navigationTargetPosition;
        }
    }
}
