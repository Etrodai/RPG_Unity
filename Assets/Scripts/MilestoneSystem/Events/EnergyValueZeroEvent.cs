using Manager;
using ResourceManagement.Manager;

namespace MilestoneSystem.Events
{
    public class EnergyValueZeroEvent : MileStoneEvent //Made by Robin
    {
        public override MileStoneEventName Name { get; set; }
        public override MileStoneEventItem[] Events { get; set; }

        private EnergyManager energyManager;

        private void Start()
        {
            energyManager = MainManagerSingleton.Instance.EnergyManager;
            Name = MileStoneEventName.EnergyValueZero;
            Events = new MileStoneEventItem[1];
            Events[0].text = "";
            Events[0].isAchieved = false;
        }

        private void Update()
        {
            if (energyManager.SavedResourceValue == 0)
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
