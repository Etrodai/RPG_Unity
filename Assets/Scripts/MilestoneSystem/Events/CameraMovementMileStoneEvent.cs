using UnityEngine.InputSystem;

namespace MilestoneSystem.Events
{
    public class CameraMovementMileStoneEvent : MileStoneEvent
    {
        #region TODOS
        
        // Anzahl, wie oft/lange bewegt werden muss hinzufügen?
        
        #endregion

        #region Variables & Properties

        public override MileStoneEventNames Name { get; set; }
        public override MileStoneEventItems[] Events { get; set; }

        #endregion

        #region UnityEvents

        /// <summary>
        /// sets variables
        /// </summary>
        private void Awake()
        {
            Name = MileStoneEventNames.CameraMovement;
            Events = new MileStoneEventItems[3];
            Events[0].text = "Move";
            Events[1].text = "Rotate";
            Events[2].text = "Zoom";
            for (int i = 0; i < Events.Length; i++)
            {
                Events[i].isAchieved = false;
            }
        }

        #endregion

        #region Events

        // TODO: (Robin) add these events to InputSystem
        
        public void Move()
        {
            Events[0].isAchieved = true;
        }

        public void Rotate()
        {
            Events[1].isAchieved = true;
        }

        public void Zoom()
        {
            Events[2].isAchieved = true;
        }

        #endregion
        
        #region Methods

        /// <summary>
        /// Checks if the given Event is achieved
        /// </summary>
        /// <param name="index">index of the event to check</param>
        /// <returns>if given event is achieved</returns>
        public override bool CheckAchieved(int index)
        {
            return Events[index].isAchieved;
        }

        /// <summary>
        /// sets all events to is not achieved
        /// </summary>
        public override void ResetAll()
        {
            for (int i = 0; i < Events.Length; i++)
            {
                Events[i].isAchieved = false;
            }
        }

        #endregion
    }
}
