using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Camera
{
    /// <summary>
    /// Script for Camera Control via Cinemachine Free View Cam
    /// Creator: Robin & Ben
    /// </summary>
    public class CameraControllerNew : MonoBehaviour
    {
        #region Variables

        [SerializeField] private CinemachineFreeLook cmFreeLook;

        //Constants
        private const float FREELOOKDISTANCE = 1000f;
        private const float MINZOOMLEVEL = 2f;
        private const float MAXZOOMLEVEL = 45f;
        private const float MINHEIGHTLEVEL = 1f;
        private const float MAXHEIGHTLEVEL = 45f;
        private const float MINSENSIVITY = .01f;
        private const float MAXSENSIVITY = 10f;
        private const float RADIUSOFFSET = 1f;

        //Axisnames
        private string XAxisName = "Mouse X";
        private string YAxisName = "Mouse Y";

        //Targets
        private Transform freeLookPoint;
        private Transform cameraLookPoint;

        //Cam
        private UnityEngine.Camera cam;

        private bool isFreeLookActive = false;
        private bool isPaused;
        private bool yInverted;

        private Vector3 camResetPos;

        private Vector3 moveVector;
        private bool isHeldDownX;
        private bool isHeldDownY;
        private float moveX;
        private float moveY;
        private Vector3 actualXAxis;

        //Sensivity
        [SerializeField, Range(0f, 10f)] private float rotationSensivity = 0.5f;
        [SerializeField, Range(0f, 10f)] private float moveSensivity = 0.5f;
        [SerializeField, Range(0f, 2f)] private float zoomSensivity = 0.5f;

        #endregion

        #region Properties
        public float RotationSensivity
        {
            get => rotationSensivity;
            set
            {
                if (value > MAXSENSIVITY)
                {
                    rotationSensivity = MAXSENSIVITY;
                    return;
                }
                else if (value < MINSENSIVITY)
                {
                    rotationSensivity = MINSENSIVITY;
                    return;
                }

                rotationSensivity = value;
            }
        }
        public float MoveSensivity
        {
            get => moveSensivity;
            set
            {
                if (value > MAXSENSIVITY)
                {
                    moveSensivity = MAXSENSIVITY;
                    return;
                }
                else if (value < MINSENSIVITY)
                {
                    moveSensivity = MINSENSIVITY;
                    return;
                }

                moveSensivity = value;
            }
        }

        public float ZoomSensivity
        {
            get => zoomSensivity;
            set
            {
                if (value > MAXSENSIVITY)
                {
                    zoomSensivity = MAXSENSIVITY;
                    return;
                }
                else if (value < MINSENSIVITY)
                {
                    zoomSensivity = MINSENSIVITY;
                    return;
                }

                zoomSensivity = value;
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
            cam = UnityEngine.Camera.main;
            cmFreeLook.m_XAxis.m_InputAxisName = "";
            cmFreeLook.m_YAxis.m_InputAxisName = "";
        }

        /// <summary>
        /// Set Targets to follow
        /// </summary>
        private void Start()
        {
            cameraLookPoint = Gridsystem.Instance.CenterTile.transform.GetChild(0); //TODO: Variable for Index
            freeLookPoint = Gridsystem.Instance.CenterTile.transform.GetChild(1);
            cmFreeLook.Follow = cameraLookPoint;
            cmFreeLook.LookAt = cameraLookPoint;

            camResetPos = cameraLookPoint.localPosition;
        }

        private void Update()
        {
            FreeLook();
            MoveXY();
        }

        #endregion

        #region Methods

        //Keyboard Button Movement
        /// <summary>
        /// Rotating Camera around Object
        /// </summary>
        /// <param name="context"></param>
        public void RotateXAxis(InputAction.CallbackContext context)
        {
            float input = context.ReadValue<float>();
            while (input != 0) //TODO: Maybe Change to something better, while isnt needed
            {
                cmFreeLook.m_XAxis.m_InputAxisValue = input * RotationSensivity;
                return;
            }

            //For Resetting Momentum
            cmFreeLook.m_XAxis.m_InputAxisValue = 0;
        }

        /// <summary>
        /// Move Camera on Y Axis around Object
        /// </summary>
        /// <param name="context"></param>
        public void RotateYAxis(InputAction.CallbackContext context)
        {
            float input = context.ReadValue<float>();
            while (input != 0)
            {
                cmFreeLook.m_YAxis.m_InputAxisValue += input * RotationSensivity;
                return;
            }

            //For Resetting Momentum
            cmFreeLook.m_YAxis.m_InputAxisValue = 0;
        }

        /// <summary>
        /// Input for XAxis
        /// </summary>
        /// <param name="context"></param>
        public void MoveCameraXAxis(InputAction.CallbackContext context)
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
        public void MoveCameraYAxis(InputAction.CallbackContext context)
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
        /// Move Cameratarget depended on MoveX and MoveY
        /// </summary>
        private void MoveXY()
        {
            if (isHeldDownX || isHeldDownY)
            {
                moveVector =
                    ((actualXAxis * moveX) + (Vector3.up * moveY)) * MoveSensivity * Time.deltaTime; //TODO: Reorder
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
        public void ResetCamera(InputAction.CallbackContext context)
        {
            cameraLookPoint.localPosition = camResetPos;
        }

        /// <summary>
        /// Active when FreeView is toggled
        /// </summary>
        private void FreeLook()
        {
            if (isPaused)
                return;

            if (isFreeLookActive && Input.GetMouseButton(0))
            {
                Vector3 mousePos = Input.mousePosition;
                freeLookPoint.transform.position =
                    cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, FREELOOKDISTANCE));
            }
        }

        //Mouse Movement
        /// <summary>
        /// For Activating Camera Axis Movement on x and y Axis
        /// </summary>
        public void ActivateCameraMovement()
        {
            if (!isFreeLookActive && !isPaused)
            {
                cmFreeLook.m_XAxis.m_InputAxisName = "Mouse X";
                cmFreeLook.m_YAxis.m_InputAxisName = "Mouse Y";
            }
        }

        /// <summary>
        /// Deactivating Camera Axis Movement on x and y Axis
        /// </summary>
        public void DeactivateCameraMovement()
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
        public void Zoom(InputAction.CallbackContext context)
        {
            if (isPaused)
                return;

            float mousewheelInput = Mathf.Clamp(context.ReadValue<float>(), -1, 1) * zoomSensivity;


            cmFreeLook.m_Orbits[1].m_Radius += mousewheelInput;

            cmFreeLook.m_Orbits[0].m_Height += mousewheelInput;
            cmFreeLook.m_Orbits[2].m_Height -= mousewheelInput;
            cmFreeLook.m_Orbits[0].m_Radius += mousewheelInput;
            cmFreeLook.m_Orbits[2].m_Radius += mousewheelInput;
        
            //Zoom Restriction
            float lowRadius = MINZOOMLEVEL - RADIUSOFFSET;
            float upRadius = MAXZOOMLEVEL - RADIUSOFFSET;
        
            if (cmFreeLook.m_Orbits[1].m_Radius < MINZOOMLEVEL)
                cmFreeLook.m_Orbits[1].m_Radius = MINZOOMLEVEL;
            else if (cmFreeLook.m_Orbits[1].m_Radius > MAXZOOMLEVEL)
                cmFreeLook.m_Orbits[1].m_Radius = MAXZOOMLEVEL;
        
            if (cmFreeLook.m_Orbits[0].m_Radius < lowRadius)
                cmFreeLook.m_Orbits[0].m_Radius = lowRadius;
            else if (cmFreeLook.m_Orbits[0].m_Radius > upRadius)
                cmFreeLook.m_Orbits[0].m_Radius = upRadius;
        
            if (cmFreeLook.m_Orbits[2].m_Radius < lowRadius)
                cmFreeLook.m_Orbits[2].m_Radius = lowRadius;
            else if (cmFreeLook.m_Orbits[2].m_Radius > upRadius)
                cmFreeLook.m_Orbits[2].m_Radius = upRadius;
        
            if (cmFreeLook.m_Orbits[0].m_Height < MINHEIGHTLEVEL)
                cmFreeLook.m_Orbits[0].m_Height = MINHEIGHTLEVEL;
            else if (cmFreeLook.m_Orbits[0].m_Height > MAXHEIGHTLEVEL)
                cmFreeLook.m_Orbits[0].m_Height = MAXHEIGHTLEVEL;

            if (cmFreeLook.m_Orbits[2].m_Height < -MAXHEIGHTLEVEL)
                cmFreeLook.m_Orbits[2].m_Height = -MAXHEIGHTLEVEL;
            else if (cmFreeLook.m_Orbits[2].m_Height > -MINHEIGHTLEVEL)
                cmFreeLook.m_Orbits[2].m_Height = -MINHEIGHTLEVEL;
        }

        //Free View
        /// <summary>
        /// Action for (de)activate Free View on Mouse
        /// </summary>
        /// <param name="context"></param>
        public void ToggleFreeLook(InputAction.CallbackContext context)
        {
            if (isPaused)
                return;

            isFreeLookActive = !isFreeLookActive;
            if (isFreeLookActive)
                cmFreeLook.LookAt = freeLookPoint;
            else
                cmFreeLook.LookAt = cameraLookPoint;
        }

//Extern Usage
        /// <summary>
        /// Option for enable Camera Movement
        /// </summary>
        public void ActivateCamera()
        {
            isPaused = false;
            ActivateCameraMovement();
        }

        /// <summary>
        /// Option for disable Camera Movement
        /// </summary>
        public void DeactivateCamera()
        {
            DeactivateCameraMovement();
            isPaused = true;
        }

        #endregion
    }
}