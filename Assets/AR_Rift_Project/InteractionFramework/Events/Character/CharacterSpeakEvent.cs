using UnityEngine;
using System.Collections;

namespace FAR
{
    /// <summary>
    /// This <see cref="CharacterEvent"/> issuses the CharacterAssistant to prestent the passed message to the user.
    /// </summary>
    public class CharacterSpeakEvent : CharacterEvent
    {
        private string m_message;

        /// <summary>
        /// The message the CharacterAssistant should present.
        /// </summary>
        public string Message
        {
            get { return m_message; }
        }

        /// <summary>
        /// Constuctor of a new CharaterSpeakEvent.
        /// </summary>
        /// <param name="message">The message the CharacterAssistant should present.</param>
        /// <param name="ts">The event's timestamp.</param>
        /// <param name="sender">The event's sender.</param>
        /// <param name="base_Evt">The event's base event.</param>
        public CharacterSpeakEvent(string message,InteractionMethod sender = null, InteractionEvent base_Evt = null):
            base(sender, base_Evt)
        {
            this.m_message=message;
        }    
    }
}
