namespace MilestoneSystem.Events
{
    public class ShowPrioritySystemMileStoneEvent : MileStoneEvent
    {
        public override MileStoneEventNames Name { get; set; }
        public override string MenuText { get; set; }
        private bool clickedPriority;
        private bool clickedPlus;
        private bool clickedMinus;
        private bool isAchieved;

        private void Awake()
        {
            Name = MileStoneEventNames.ShowPrioritySystem;
            MenuText = "Click on the PrioritySystemButton.\nChange the Priority up and down.";
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
