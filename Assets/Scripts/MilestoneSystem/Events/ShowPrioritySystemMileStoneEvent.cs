namespace MilestoneSystem.Events
{
    public class ShowPrioritySystemMileStoneEvent : MileStoneEvent //Made by Robin
    {
        #region Variables & Properties

        public override MileStoneEventName Name { get; set; }
        public override MileStoneEventItem[] Events { get; set; }

        #endregion

        #region UnityEvents

        /// <summary>
        /// sets variables
        /// </summary>
        private void Awake()
        {
            Name = MileStoneEventName.ShowPrioritySystem;
            Events = new MileStoneEventItem[3];
            Events[0].text = "Öffne das Prio-Menü";
            Events[1].text = "Verändere die Priorität nach oben";
            Events[2].text = "Verändere die Priorität nach unten";
            Events[0].isAchieved = false;
            Events[1].isAchieved = false;
            Events[2].isAchieved = false;
        }

        #endregion

        #region Events

        public void ClickPriorityButton()
        {
            Events[0].isAchieved = true;
        }

        public void ClickPlusButton()
        {
            Events[1].isAchieved = true;
        }

        public void ClickMinusButton()
        {
            Events[2].isAchieved = true;
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
            for (int i = 0; i < Events.Length; i++)
            {
                Events[i].isAchieved = false;
            }
        }

        #endregion
    }
}
