using UnityEngine;
using System.Collections;
namespace FAR
{
    public class CharakterObjectEvent : CharacterEvent
    {

        private Pose m_pose;

        /// <summary>
        /// The message the CharacterAssistant should present.
        /// </summary>
        public Pose UBIPOSE
        {
            get { return m_pose; }
        }

        /// <summary>
        /// Constuctor of a new CharaterSpeakEvent.
        /// </summary>
        /// <param name="message">The message the CharacterAssistant should present.</param>
        /// <param name="ts">The event's timestamp.</param>
        /// <param name="sender">The event's sender.</param>
        /// <param name="base_Evt">The event's base event.</param>
        public CharakterObjectEvent(FAR.Pose pose, InteractionMethod sender = null, InteractionEvent base_Evt = null) :
            base(sender, base_Evt)
        {
            this.m_pose=pose;
        }    
    }
}