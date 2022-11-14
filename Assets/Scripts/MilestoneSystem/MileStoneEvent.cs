using System;
using System.Collections.Generic;
using UnityEngine;

namespace MilestoneSystem
{
    public abstract class MileStoneEvent : MonoBehaviour
    {
        public abstract MileStoneEventNames Name { get; set; }
        public abstract MileStoneEventItems[] Events { get; set; }
        
        /// <summary>
        /// Checks if the given Event is achieved
        /// </summary>
        /// <param name="index">index of the event to check</param>
        /// <returns>if given event is achieved</returns>
        public abstract bool CheckAchieved(int index);
        
        /// <summary>
        /// sets all events to is not achieved
        /// </summary>
        public abstract void ResetAll();
    }
}
