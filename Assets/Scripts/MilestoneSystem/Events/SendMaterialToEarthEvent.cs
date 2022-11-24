using Manager;
using ResourceManagement.Manager;

namespace MilestoneSystem.Events
{
    public class SendMaterialToEarthEvent : MileStoneEvent
    {
        public override MileStoneEventName Name { get; set; }
        public override MileStoneEventItem[] Events { get; set; }
        private bool isActive;
        private MaterialManager materialManager;
        private void Start()
        {
            materialManager = MainManagerSingleton.Instance.MaterialManager;
            Name = MileStoneEventName.SendMaterialToEarth;
            Events = new MileStoneEventItem[1];
            Events[0].text = "";
            Events[0].isAchieved = false;
        }
        
        private void Update()
        {
            if (!isActive) return;
            materialManager.SavedResourceValue -= 3000;
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
