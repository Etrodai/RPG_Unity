using UnityEngine;

namespace MilestoneSystem
{
    public abstract class MileStoneEvent : MonoBehaviour //Made by Robin
    {
        public abstract MileStoneEventName Name { get; set; }
        public abstract MileStoneEventItem[] Events { get; set; }
        
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
