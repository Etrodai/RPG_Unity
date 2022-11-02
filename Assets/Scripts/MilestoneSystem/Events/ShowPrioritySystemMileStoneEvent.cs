using System.Collections.Generic;

namespace MilestoneSystem.Events
{
    public class ShowPrioritySystemMileStoneEvent : MileStoneEvent
    {
        public override MileStoneEventNames Name { get; set; }
        public override List<string> MenuText { get; set; } = new();
        private bool clickedPriority;
        private bool clickedPlus;
        private bool clickedMinus;
        private bool isAchieved;

        private void Awake()
        {
            Name = MileStoneEventNames.ShowPrioritySystem;
            MenuText.Add("Click on the Priority-System-Button");
            MenuText.Add("Change the Priority up and down");
        }

        public override bool CheckAchieved()
        {
            return isAchieved;
        }
        
        public override void ResetAll()
        {
            clickedPriority = false;
            clickedPlus = false;
            clickedMinus = false;
            isAchieved = false;
        }

        #region Events

        public void ClickPriorityButton()
        {
            clickedPriority = true;
            if (clickedPriority && clickedPlus && clickedMinus)
            {
                isAchieved = true;
            }
        }

        public void ClickPlusButton()
        {
            clickedPlus = true;
            if (clickedPriority && clickedPlus && clickedMinus)
            {
                isAchieved = true;
            }
        }

        public void ClickMinusButton()
        {
            clickedMinus = true;
            if (clickedPriority && clickedPlus && clickedMinus)
            {
                isAchieved = true;
            }
        }
        
        #endregion
    }
}
