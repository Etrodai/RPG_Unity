using UnityEngine;
using UnityEngine.InputSystem;

namespace MilestoneSystem.Events
{
    public class CameraMovementMileStoneEvent : MileStoneEvent //Made by Robin
    {
        #region Variables & Properties

        //Input
        [SerializeField] private PlayerInput playerInput;
        private float movingTimer = 2f;
        private float movingTime;
        private float zoomCounter = 3;
        private float rotateTimer = 1f;
        private float rotateTime;
        public override MileStoneEventName Name { get; set; }
        public override MileStoneEventItem[] Events { get; set; }

        #endregion

        #region UnityEvents

        /// <summary>
        /// sets variables
        /// </summary>
        private void Awake()
        {
            Name = MileStoneEventName.CameraMovement;
            Events = new MileStoneEventItem[3];
            Events[0].text = "Bewegen";
            Events[1].text = "Rotieren";
            Events[2].text = "Zoomen";
            for (int i = 0; i < Events.Length; i++)
            {
                Events[i].isAchieved = false;
            }
        }

        #endregion

        #region Events

        private void Move(InputAction.CallbackContext context)
        {
            if (context.started) movingTime = Time.time;

            if (!context.canceled) return;
            movingTime = Time.time - movingTime;
            movingTimer -= movingTime;

            if (!(movingTimer <= 0)) return;
            Events[0].isAchieved = true;

            playerInput.actions["MoveXAxis"].started -= Move;
            playerInput.actions["MoveXAxis"].canceled -= Move;
            playerInput.actions["MoveYAxis"].started -= Move;
            playerInput.actions["MoveYAxis"].canceled -= Move;


        }

        private void Rotate(InputAction.CallbackContext context)
        {
            if (context.started) rotateTime = Time.time;
            
            if (!context.canceled) return;
            rotateTime = Time.time - rotateTime;
            rotateTimer -= rotateTime;

            if (!(rotateTimer <= 0)) return;
            Events[1].isAchieved = true;
            playerInput.actions["ActivateCameraMovement"].started -= Move;
            playerInput.actions["ActivateCameraMovement"].canceled -= Move;
            playerInput.actions["RotateXAxis"].started -= Rotate;
            playerInput.actions["RotateXAxis"].canceled -= Rotate;
            playerInput.actions["RotateYAxis"].started -= Rotate;
            playerInput.actions["RotateYAxis"].canceled -= Rotate;
        }

        private void Zoom(InputAction.CallbackContext context)
        {
            zoomCounter--;
            if (!(zoomCounter <= 0)) return;
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
            playerInput.actions["MoveXAxis"].started += Move;
            playerInput.actions["MoveXAxis"].canceled += Move;
            playerInput.actions["MoveYAxis"].started += Move;
            playerInput.actions["MoveYAxis"].canceled += Move;
            playerInput.actions["RotateXAxis"].started += Rotate;
            playerInput.actions["RotateXAxis"].canceled += Rotate;
            playerInput.actions["RotateYAxis"].started += Rotate;
            playerInput.actions["RotateYAxis"].canceled += Rotate;
            playerInput.actions["ActivateCameraMovement"].started += Rotate;
            playerInput.actions["ActivateCameraMovement"].canceled += Rotate;
            playerInput.actions["Zoom"].performed += Zoom;
        }

        #endregion
    }
}
