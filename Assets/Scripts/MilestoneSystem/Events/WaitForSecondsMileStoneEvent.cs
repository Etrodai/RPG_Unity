using System.Collections.Generic;
using UnityEngine;

namespace MilestoneSystem.Events
{
    public class WaitForSecondsMileStoneEvent : MileStoneEvent
    {
        public override MileStoneEventNames Name { get; set; }
        public override List<string> MenuText { get; set; } = new();
        private bool isAchieved;
        private float timer = 10f;

        private void Start()
        {
            Name = MileStoneEventNames.WaitForSeconds;
        }

        void Update()
        {
            if (isAchieved) return;
            if (timer <= 0) isAchieved = true;
            else timer -= Time.deltaTime;
        }
    
        public override void ResetAll()
        {
            timer = 10f;
            isAchieved = false;
        }
    
        public override bool CheckAchieved()
        {
            return isAchieved;
        }
    }
}
