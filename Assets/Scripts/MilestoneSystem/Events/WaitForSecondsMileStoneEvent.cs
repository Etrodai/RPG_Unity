using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MilestoneSystem.Events
{
    public class WaitForSecondsMileStoneEvent : MileStoneEvent
    {
        #region Variables & Properties

        private float timer = 10f;
        public override MileStoneEventNames Name { get; set; }
        public override MileStoneEventItems[] Events { get; set; }

        #endregion

        #region MyRegion

        /// <summary>
        /// sets variables
        /// </summary>
        private void Start()
        {
            Name = MileStoneEventNames.WaitForSeconds;
            Events = new MileStoneEventItems[1];
            Events[0].text = "";
            Events[0].isAchieved = false;
        }

        /// <summary>
        /// updates countdown
        /// </summary>
        void Update()
        {
            if (Events[0].isAchieved) return;
            if (timer <= 0) Events[0].isAchieved = true;
            else timer -= Time.deltaTime;
        }

        #endregion
        
        #region Methods
        
        /// <summary>
        /// Checks if the given Event is achieved
        /// </summary>
        /// <param name="index">index of the event to check</param>
        /// <returns>if given event is achieved</returns>
        public override bool CheckAchieved(int index)
        {
            return Events[index].isAchieved;
        }
        
        /// <summary>
        /// sets all events to is not achieved
        /// </summary>
        public override void ResetAll()
        {
            timer = 10f;
            Events[0].isAchieved = false;
        }
        
        #endregion
    }
}
