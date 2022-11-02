using System.Collections.Generic;
using UnityEngine;

namespace MilestoneSystem
{
    public abstract class MileStoneEvent : MonoBehaviour
    {
        public abstract MileStoneEventNames Name { get; set; }
        public abstract List<string> MenuText { get; set; }
        public abstract bool CheckAchieved();
        public abstract void ResetAll();
    }
}
