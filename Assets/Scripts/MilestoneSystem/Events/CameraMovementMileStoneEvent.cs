using Unity.VisualScripting;

namespace MilestoneSystem.Events
{
    public class CameraMovementMileStoneEvent : MileStoneEvent
    {
        #region TODOS
        
        // Anzahl, wie oft/lange bewegt werden muss hinzufügen?
        
        #endregion
        
        public override MileStoneEventNames Name { get; set; }
        public override string MenuText { get; set; }
        private bool hasMoved;
        private bool hasRotated;
        private bool hasZoomed;
        private bool isAchieved;

        private void Awake()
        {
            Name = MileStoneEventNames.CameraMovement;
            MenuText = "Move, Rotate and Zoom.";
        }

        public override bool CheckAchieved()
        {
            return isAchieved;
        }

        public override void ResetAll()
        {
            hasMoved = false;
            hasRotated = false;
            hasZoomed = false;
            isAchieved = false;
        }

        #region Events

        public void Move()
        {
            hasMoved = true;
            if (hasMoved && hasRotated && hasZoomed)
            {
                isAchieved = true;
            }
        }

        public void Rotate()
        {
            hasRotated = true;
            if (hasMoved && hasRotated && hasZoomed)
            {
                isAchieved = true;
            }
        }

        public void Zoom()
        {
            hasZoomed = true;
            if (hasMoved && hasRotated && hasZoomed)
            {
                isAchieved = true;
            }
        }

        #endregion
    }
}
