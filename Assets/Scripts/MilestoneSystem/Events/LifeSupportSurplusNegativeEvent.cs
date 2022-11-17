using System;
using Manager;
using ResourceManagement.Manager;

namespace MilestoneSystem.Events
{
    public class LifeSupportSurplusNegativeEvent : MileStoneEvent
    {
        public override MileStoneEventNames Name { get; set; }
        public override MileStoneEventItems[] Events { get; set; }

        private FoodManager foodManager;
        private WaterManager waterManager;

        private void Start()
        {
            foodManager = MainManagerSingleton.Instance.FoodManager;
            waterManager = MainManagerSingleton.Instance.WaterManager;
            Name = MileStoneEventNames.LifeSupportSurplusNegative;
            Events = new MileStoneEventItems[1];
            Events[0].text = "";
            Events[0].isAchieved = false;
        }

        private void Update()
        {
            if (foodManager.CurrentResourceSurplus <= 0 || waterManager.CurrentResourceSurplus <= 0)
            {
                Events[0].isAchieved = true;
            }
        }

        public override bool CheckAchieved(int index)
        {
            return Events[index].isAchieved;
        }

        public override void ResetAll()
        {
            Events[0].isAchieved = false;
        }
    }
}
