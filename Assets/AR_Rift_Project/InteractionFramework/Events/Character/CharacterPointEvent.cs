using UnityEngine;
using System.Collections;

namespace FAR
{
    /// <summary>
    /// This <see cref="CharacterEvent"/> issues the CharacterAssistant to point and to look at a specific location.
    /// </summary>
    public class CharacterPointEvent : CharacterEvent
    {
        private Transform m_pointingTarget;
        private Transform m_lookingTarget;
        private PointingArm m_pointingArm;

        /// <summary>
        /// This property returns the target the CharacerAssistant should point at.
        /// </summary>
        public Transform PointingTarget
        {
            get { return m_pointingTarget; }
        }

        /// <summary>
        /// This property returns which arm the CharacterAssistant should use for pointing.
        /// </summary>
        public PointingArm PointingArm
        {
            get { return m_pointingArm; }
        }

        /// <summary>
        /// This property return the traget the CharacterAssistant should look at.
        /// </summary>
        public Transform LookingTarget 
        { 
            get { return m_lookingTarget; } 
        }

        /// <summary>
        /// Construts a CharacterPointEvent. 
        /// </summary>
        /// <param name="pointingTarget">The target the CharacterAssistant should point at.</param>
        /// <param name="lookingTarget">The target the CharacterAssistant schould look at.</param>
        /// <param name="pointingArm">The arm the CharacterAssistant should use.</param>
        /// <param name="ts">The timestamp of the event.</param>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="base_Evt">The base event.</param>
        public CharacterPointEvent(Transform pointingTarget, Transform lookingTarget, PointingArm pointingArm,InteractionMethod sender = null, InteractionEvent base_Evt = null)
            :base(sender,base_Evt)
        {
            this.m_pointingTarget = pointingTarget;
            this.m_lookingTarget = lookingTarget;
            this.m_pointingArm = pointingArm;
        }

    }

    /// <summary>
    /// This enum is used by <see cref="CharacterPointEvent"/>. 
    /// It indicates which arm of the Avatar should point to the target.
    /// </summary>
    public enum PointingArm { Left,Right}
}
