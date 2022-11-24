using System;
using ResourceManagement.Manager;

namespace MilestoneSystem.Events
{
    public class SendFoodToEarthEvent : MileStoneEvent
    {
        public override MileStoneEventName Name { get; set; }
        public override MileStoneEventItem[] Events { get; set; }
        private bool isActive;
        private FoodManager foodManager;
        private WaterManager waterManager;
        
        private void Start()
        {
            Name = MileStoneEventName.SendFoodToEarth;
            Events = new MileStoneEventItem[1];
            Events[0].text = "";
            Events[0].isAchieved = false;
        }
        
        private void Update()
        {
            if (!isActive) return;
            foodManager.SavedResourceValue -= 500;
            waterManager.SavedResourceValue -= 500;
            Events[0].isAchieved = true;
            isActive = false;
        }

        public override bool CheckAchieved(int index)
        {
            return Events[index].isAchieved;
        }

        public override void ResetAll()
        {
            Events[0].isAchieved = false;
            isActive = true;
        }
    }
}
