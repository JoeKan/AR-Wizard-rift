using UnityEngine;
using System.Collections;

namespace FAR
{
    /// <summary>
    /// This abstract <see cref="InteractionEvent"/> is the base event of all events that are used to controll the 
    /// character assistant.
    /// </summary>
    public abstract class CharacterEvent :  InteractionEvent
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public CharacterEvent() { }

        /// <summary>
        /// Default contructor. No additional Functionality here.
        /// </summary>
        /// <param name="ts">The event's timestamp</param>
        /// <param name="sender">The Sender of this event</param>
        /// <param name="base_Evt">The base Event of this event</param>
        public CharacterEvent(InteractionMethod sender = null, InteractionEvent base_Evt = null)
            : base(sender, base_Evt)
        {

        }

    }

}
