using Cinemachine;
using UI.Gridsystem;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Cameras
{
    /// <summary>
    /// Script for Camera Control via Cinemachine Free View Cam
    /// Creator: Robin & Ben
    /// </summary>
    public class CameraControllerNew : MonoBehaviour //Made by Ben
    {
        #region Variables

        private const int CHILDINDEX = 0;
        
        [SerializeField] private CinemachineFreeLook cmFreeLook;

        //Constants
        private const float FreeLookDistance = 1000f;
        private const float MinZoomLevel = 2f;
        private const float MaxZoomLevel = 45f;
        private const float MinHeightLevel = 1f;
        private const float MaxHeightLevel = 45f;
        private const float MinSensitivity = .01f;
        private const float MaxSensitivity = 10f;
        private const float RadiusOffset = 1f;

        //Axis names
        private string xAxisName = "Mouse X"; //TODO: is it used???
        private string yAxisName = "Mouse Y"; //TODO: is it used???

        //Targets
        private Transform freeLookPoint;
        private Transform cameraLookPoint;

        //Cam
        private Camera cam;
        
        private bool isPaused;
        private bool yInverted;

        private Vector3 camResetPos;

        private Vector3 moveVector;
        private bool isHeldDownX;
        private bool isHeldDownY;
        private float moveX;
        private float moveY;
        private Vector3 actualXAxis;

        //Sensitivity
        [SerializeField, Range(0f, 10f)] private float rotationSensitivity = 0.5f;
        [SerializeField, Range(0f, 10f)] private float moveSensitivity = 0.5f;
        [SerializeField, Range(0f, 2f)] private float zoomSensitivity = 0.5f;
        
        //Input
        [SerializeField] private PlayerInput playerInput;
        private bool playerInputHasBeenInit;

        #endregion

        #region Properties
        public float RotationSensitivity
        {
            get => rotationSensitivity;
            set
            {
                if (value > MaxSensitivity)
                {
                    rotationSensitivity = MaxSensitivity;
                    return;
                }
                else if (value < MinSensitivity)
                {
                    rotationSensitivity = MinSensitivity;
                    return;
                }

                rotationSensitivity = value;
            }
        }
        public float MoveSensitivity
        {
            get => moveSensitivity;
            set
            {
                if (value > MaxSensitivity)
                {
                    moveSensitivity = MaxSensitivity;
                    return;
                }
                else if (value < MinSensitivity)
                {
                    moveSensitivity = MinSensitivity;
                    return;
                }

                moveSensitivity = value;
            }
        }

        public float ZoomSensitivity
        {
            get => zoomSensitivity;
            set
            {
                if (value > MaxSensitivity)
                {
                    zoomSensitivity = MaxSensitivity;
                    return;
                }
                else if (value < MinSensitivity)
                {
                    zoomSensitivity = MinSensitivity;
                    return;
                }

                zoomSensitivity = value;
            }
        }

        #endregion

        #region Unity Events

        /// <summary>
        /// Get Target and delete defined Names for Axis
        /// </summary>
        private void Awake()
        {
            cmFreeLook = this.GetComponent<CinemachineFreeLook>();
            cam = Camera.main;
            cmFreeLook.m_XAxis.m_InputAxisName = "";
            cmFreeLook.m_YAxis.m_InputAxisName = "";
        }

        /// <summary>
        /// Set Targets to follow
        /// </summary>
        private void Start()
        {
            cameraLookPoint = Gridsystem.Instance.CenterTile.transform.GetChild(CHILDINDEX);
            cmFreeLook.Follow = cameraLookPoint;
            cmFreeLook.LookAt = cameraLookPoint;

            camResetPos = cameraLookPoint.localPosition;
        }

        private void Update()
        {
            if (!playerInputHasBeenInit)
            {
                InitPlayerInput();
            }
            
            MoveXY();
        }

        private void OnDisable()
        {
            if (playerInput == null) return;
            playerInput.actions["Reset Camera"].performed -= ResetCamera;
            playerInput.actions["DeactivateCameraMovement"].performed -= DeactivateCameraMovement;
            playerInput.actions["ActivateCameraMovement"].performed -= ActivateCameraMovement;
            playerInput.actions["MoveYAxis"].performed -= MoveCameraYAxis;
            playerInput.actions["MoveYAxis"].canceled -= MoveCameraYAxis;
            playerInput.actions["MoveXAxis"].performed -= MoveCameraXAxis;
            playerInput.actions["MoveXAxis"].canceled -= MoveCameraXAxis;
            playerInput.actions["Zoom"].performed -= Zoom;
            playerInput.actions["RotateXAxis"].performed -= RotateXAxis;
            playerInput.actions["RotateXAxis"].canceled -= RotateXAxis;
            playerInput.actions["RotateYAxis"].performed -= RotateYAxis; 
            playerInput.actions["RotateYAxis"].canceled -= RotateYAxis;
            playerInputHasBeenInit = false;
        }

        #endregion

        #region Methods

        private void InitPlayerInput()
        {
            if (!playerInput.isActiveAndEnabled) return;
            playerInput.actions["Reset Camera"].performed += ResetCamera;
            playerInput.actions["DeactivateCameraMovement"].performed += DeactivateCameraMovement;
            playerInput.actions["ActivateCameraMovement"].performed += ActivateCameraMovement;
            playerInput.actions["MoveYAxis"].performed += MoveCameraYAxis;
            playerInput.actions["MoveYAxis"].canceled += MoveCameraYAxis;
            playerInput.actions["MoveXAxis"].performed += MoveCameraXAxis;
            playerInput.actions["MoveXAxis"].canceled += MoveCameraXAxis;
            playerInput.actions["Zoom"].performed += Zoom;
            playerInput.actions["RotateXAxis"].performed += RotateXAxis;
            playerInput.actions["RotateXAxis"].canceled += RotateXAxis;
            playerInput.actions["RotateYAxis"].performed += RotateYAxis;
            playerInput.actions["RotateYAxis"].canceled += RotateYAxis;
            playerInputHasBeenInit = true;
        }

        //Keyboard Button Movement
        /// <summary>
        /// Rotating Camera around Object
        /// </summary>
        /// <param name="context"></param>
        private void RotateXAxis(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                float input = context.ReadValue<float>();
                cmFreeLook.m_XAxis.m_InputAxisValue = input * RotationSensitivity;
            }

            if (context.canceled)
            {
                //For Resetting Momentum
                cmFreeLook.m_XAxis.m_InputAxisValue = 0;
            }
        }

        /// <summary>
        /// Move Camera on Y Axis around Object
        /// </summary>
        /// <param name="context"></param>
        private void RotateYAxis(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                float input = context.ReadValue<float>();
                cmFreeLook.m_YAxis.m_InputAxisValue += input * RotationSensitivity;
            }

            if (context.canceled)
            {
                //For Resetting Momentum
                cmFreeLook.m_YAxis.m_InputAxisValue = 0;
            }
        }

        /// <summary>
        /// Input for XAxis
        /// </summary>
        /// <param name="context"></param>
        private void MoveCameraXAxis(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                isHeldDownX = true;
                actualXAxis = cam.transform.right;
            }

            if (context.canceled)
            {
                isHeldDownX = false;
                moveX = 0;
                actualXAxis = Vector3.zero;
            }

            moveX = context.ReadValue<float>();
        }

        /// <summary>
        /// Input for Y Axis
        /// </summary>
        /// <param name="context"></param>
        private void MoveCameraYAxis(InputAction.CallbackContext context)
        {
            if (context.performed)
                isHeldDownY = true;
            if (context.canceled)
            {
                isHeldDownY = false;
                moveY = 0;
            }

            moveY = context.ReadValue<float>();
        }

        /// <summary>
        /// Move Camera target depended on MoveX and MoveY
        /// </summary>
        private void MoveXY()
        {
            if (isHeldDownX || isHeldDownY)
            {
                moveVector =
                    ((actualXAxis * moveX) + (Vector3.up * moveY)) * (MoveSensitivity * Time.deltaTime); //TODO: (Ben) Reorder
                cameraLookPoint.localPosition += moveVector;
            }

            if (isHeldDownX && isHeldDownY)
            {
                moveVector = Vector3.zero;
            }
        }

        /// <summary>
        /// Resetting Camera to Central Point
        /// </summary>
        /// <param name="context"></param>
        private void ResetCamera(InputAction.CallbackContext context)
        {
            cameraLookPoint.localPosition = camResetPos;
        }

        //Mouse Movement
        /// <summary>
        /// For Activating Camera Axis Movement on x and y Axis
        /// </summary>
        private void ActivateCameraMovement(InputAction.CallbackContext context)
        {
            if (!isPaused)
            {
                cmFreeLook.m_XAxis.m_InputAxisName = "Mouse X";
                cmFreeLook.m_YAxis.m_InputAxisName = "Mouse Y";
            }
        }

        /// <summary>
        /// Deactivating Camera Axis Movement on x and y Axis
        /// </summary>
        private void DeactivateCameraMovement(InputAction.CallbackContext context)
        {
            cmFreeLook.m_XAxis.m_InputAxisName = "";
            cmFreeLook.m_YAxis.m_InputAxisName = "";

            cmFreeLook.m_XAxis.m_InputAxisValue = 0;
            cmFreeLook.m_YAxis.m_InputAxisValue = 0;
        }

        /// <summary>
        /// Get Camera near or far from Object
        /// </summary>
        /// <param name="context"></param>
        private void Zoom(InputAction.CallbackContext context)
        {
            if (isPaused)
                return;

            float mousewheelInput = Mathf.Clamp(context.ReadValue<float>(), -1, 1) * zoomSensitivity;


            cmFreeLook.m_Orbits[1].m_Radius += mousewheelInput;

            cmFreeLook.m_Orbits[0].m_Height += mousewheelInput;
            cmFreeLook.m_Orbits[2].m_Height -= mousewheelInput;
            cmFreeLook.m_Orbits[0].m_Radius += mousewheelInput;
            cmFreeLook.m_Orbits[2].m_Radius += mousewheelInput;
        
            //Zoom Restriction
            float lowRadius = MinZoomLevel - RadiusOffset;
            float upRadius = MaxZoomLevel - RadiusOffset;
        
            if (cmFreeLook.m_Orbits[1].m_Radius < MinZoomLevel)
                cmFreeLook.m_Orbits[1].m_Radius = MinZoomLevel;
            else if (cmFreeLook.m_Orbits[1].m_Radius > MaxZoomLevel)
                cmFreeLook.m_Orbits[1].m_Radius = MaxZoomLevel;
        
            if (cmFreeLook.m_Orbits[0].m_Radius < lowRadius)
                cmFreeLook.m_Orbits[0].m_Radius = lowRadius;
            else if (cmFreeLook.m_Orbits[0].m_Radius > upRadius)
                cmFreeLook.m_Orbits[0].m_Radius = upRadius;
        
            if (cmFreeLook.m_Orbits[2].m_Radius < lowRadius)
                cmFreeLook.m_Orbits[2].m_Radius = lowRadius;
            else if (cmFreeLook.m_Orbits[2].m_Radius > upRadius)
                cmFreeLook.m_Orbits[2].m_Radius = upRadius;
        
            if (cmFreeLook.m_Orbits[0].m_Height < MinHeightLevel)
                cmFreeLook.m_Orbits[0].m_Height = MinHeightLevel;
            else if (cmFreeLook.m_Orbits[0].m_Height > MaxHeightLevel)
                cmFreeLook.m_Orbits[0].m_Height = MaxHeightLevel;

            if (cmFreeLook.m_Orbits[2].m_Height < -MaxHeightLevel)
                cmFreeLook.m_Orbits[2].m_Height = -MaxHeightLevel;
            else if (cmFreeLook.m_Orbits[2].m_Height > -MinHeightLevel)
                cmFreeLook.m_Orbits[2].m_Height = -MinHeightLevel;
        }

        //Extern Usage
        /// <summary>
        /// Option for enable Camera Movement
        /// </summary>
        public void ActivateCamera()
        {
            isPaused = false;
            ActivateCameraMovement(new InputAction.CallbackContext());
        }

        /// <summary>
        /// Option for disable Camera Movement
        /// </summary>
        public void DeactivateCamera()
        {
            DeactivateCameraMovement(new InputAction.CallbackContext());
            isPaused = true;
        }

        public void InvertYAxis(bool invertState)
        {
            cmFreeLook.m_YAxis.m_InvertInput = invertState;
        }
        
        #endregion
    }
}