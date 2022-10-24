using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

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
    private const float MINZOOMLEVEL = 3f;
    private const float MAXZOOMLEVEL = 45f;
    private const float MINHEIGHTLEVEL = 4.5f;
    private const float MAXHEIGHTLEVEL = 47f;
    private const float MINSENSIVITY = .01f;
    private const float MAXSENSIVITY = 2f;

    //Axisnames
    private string XAxisName = "Mouse X";
    private string YAxisName = "Mouse Y";

    //Targets
    private Transform freeLookPoint;
    private Transform cameraLookPoint;

    //Cam
    private Camera cam;
    private bool isFreeLookActive = false;
    private bool isPaused;

    //Sensivity
    [SerializeField, Range(0f, 1f)] private float rotationSensivity = 0.5f;
    private float radiusOffset = -2f;
    private float heightSensivity = 0.5f;

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
        cameraLookPoint = Gridsystem.Instance.CenterTile.transform.GetChild(0); //TODO: Variable for Index
        freeLookPoint = Gridsystem.Instance.CenterTile.transform.GetChild(1);
        cmFreeLook.Follow = cameraLookPoint;
        cmFreeLook.LookAt = cameraLookPoint;
    }

    private void Update()
    {
        FreeLook();
    }

    #endregion

    #region Methods

    //Keyboard Button Movement
    /// <summary>
    /// Rotating Camera around Object
    /// </summary>
    /// <param name="context"></param>
    public void Rotate(InputAction.CallbackContext context)
    {
        float input = context.ReadValue<float>();
        while (input != 0)
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
    public void Move(InputAction.CallbackContext context)
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

        float mousewheelInput = Mathf.Clamp(context.ReadValue<float>(), -1, 1);

        cmFreeLook.m_Orbits[1].m_Radius += mousewheelInput;

        cmFreeLook.m_Orbits[0].m_Height += mousewheelInput;
        cmFreeLook.m_Orbits[2].m_Height -= mousewheelInput;

        //Zoom Restriction
        if (cmFreeLook.m_Orbits[1].m_Radius < MINZOOMLEVEL)
            cmFreeLook.m_Orbits[1].m_Radius = MINZOOMLEVEL;
        else if (cmFreeLook.m_Orbits[1].m_Radius > MAXZOOMLEVEL)
            cmFreeLook.m_Orbits[1].m_Radius = MAXZOOMLEVEL;

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