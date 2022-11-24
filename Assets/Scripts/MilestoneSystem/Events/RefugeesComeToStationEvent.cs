using System;
using Manager;
using ResourceManagement.Manager;

namespace MilestoneSystem.Events
{
    public class RefugeesComeToStationEvent : MileStoneEvent
    {
        public override MileStoneEventName Name { get; set; }
        public override MileStoneEventItem[] Events { get; set; }
        private bool isActive;
        private CitizenManager citizenManager;

        private void Start()
        {
            citizenManager = MainManagerSingleton.Instance.CitizenManager;
            Name = MileStoneEventName.RefugeesComeToStation;
            Events = new MileStoneEventItem[1];
            Events[0].text = "";
            Events[0].isAchieved = false;
        }

        private void Update()
        {
            if (!isActive) return;
            citizenManager.CurrentResourceProduction = citizenManager.SaveSpace;
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
