using UnityEngine;
using UnityEngine.InputSystem;

namespace MilestoneSystem.Events
{
    public class CameraMovementMileStoneEvent : MileStoneEvent
    {
        #region TODOS
        
        // Anzahl, wie oft/lange bewegt werden muss hinzufügen?
        
        #endregion

        #region Variables & Properties

        //Input
        [SerializeField] private PlayerInput playerInput;

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

        private void Move(InputAction.CallbackContext context)
        {
            Events[0].isAchieved = true;
            playerInput.actions["ActivateCameraMovement"].performed -= Move;
            playerInput.actions["MoveXAxis"].performed -= Move;
            playerInput.actions["MoveYAxis"].performed -= Move;
        }

        private void Rotate(InputAction.CallbackContext context)
        {
            Events[1].isAchieved = true;
            playerInput.actions["RotateXAxis"].performed -= Rotate;
            playerInput.actions["RotateYAxis"].performed -= Rotate;
        }

        private void Zoom(InputAction.CallbackContext context)
        {
            Events[2].isAchieved = true;
            playerInput.actions["Zoom"].performed -= Zoom;
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
            
            InitPlayerInput();
        }
        
        private void InitPlayerInput()
        {
            playerInput.actions["ActivateCameraMovement"].performed += Move;
            playerInput.actions["MoveXAxis"].performed += Move;
            playerInput.actions["MoveYAxis"].performed += Move;
            playerInput.actions["RotateXAxis"].performed += Rotate;
            playerInput.actions["RotateYAxis"].performed += Rotate;
            playerInput.actions["Zoom"].performed += Zoom;
        }

        #endregion
    }
}
