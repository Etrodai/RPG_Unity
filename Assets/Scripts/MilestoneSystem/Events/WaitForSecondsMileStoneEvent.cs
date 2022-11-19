using UnityEngine;

namespace MilestoneSystem.Events
{
    public class WaitForSecondsMileStoneEvent : MileStoneEvent
    {
        #region Variables & Properties

        [SerializeField] private float timerStart = 10f;
        private float timer;
        public override MileStoneEventName Name { get; set; }
        public override MileStoneEventItem[] Events { get; set; }

        #endregion

        #region UnityEvents

        /// <summary>
        /// sets variables
        /// </summary>
        private void Start()
        {
            Name = MileStoneEventName.WaitForSeconds;
            Events = new MileStoneEventItem[1];
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
            timer = timerStart;
            Events[0].isAchieved = false;
        }
        
        #endregion
    }
}
