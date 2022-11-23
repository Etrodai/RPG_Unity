using Eventsystem;
using MilestoneSystem;
using PriorityListSystem;

namespace SaveSystem
{
    [System.Serializable]
    public class GameSave //Made by Robin
    {
        public GridSystemData[] gridData;
        public OrbitingData[] orbitingData;
        public float[] managerData;
        public PriorityListItemSave[] priorityListData;
        public MileStoneSystemSave mileStoneData;
        public EventManagerSave eventData;
    }
}
