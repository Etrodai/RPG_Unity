using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class OldOptionsMenu : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown resolution;
    [SerializeField] private TextMeshProUGUI resolutionText;
    [SerializeField] private TMP_Dropdown windowMode;
    [SerializeField] private TextMeshProUGUI windowModeText;


    private int screenWidth = 1920;
    private int screenHeight = 1080;

    private void Start()
    {
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        resolutionText.text = $"{screenWidth}x{screenHeight}";

        switch (Screen.fullScreenMode)
        {
            case FullScreenMode.ExclusiveFullScreen:
                windowModeText.text = "Vollbild";
                break;
            case FullScreenMode.FullScreenWindow:
                windowModeText.text = "Rahmenloses Fenster";
                break;
            case FullScreenMode.Windowed:
                windowModeText.text = "Fenster";
                break;
            default:
                break;
        }
    }

    public void EnableDisableMenu(InputAction.CallbackContext context)
    {
        if (gameObject.activeInHierarchy == true && context.performed)
        {
            gameObject.SetActive(false);
        }
    }

    #region Graphic options
    public void SetResolution()
    {
        switch (resolution.value)
        {
            case 0:
                screenWidth = 2560;
                screenHeight = 1440;
                break;
            case 1:
                screenWidth = 1920;
                screenHeight = 1200;
                break;
            case 2:
                screenWidth = 1920;
                screenHeight = 1080;
                break;
            case 3:
                screenWidth = 1680;
                screenHeight = 1050;
                break;
            case 4:
                screenWidth = 1440;
                screenHeight = 900;
                break;
            case 5:
                screenWidth = 1366;
                screenHeight = 768;
                break;
            case 6:
                screenWidth = 1280;
                screenHeight = 800;
                break;
            case 7:
                screenWidth = 1280;
                screenHeight = 720;
                break;
            case 8:
                screenWidth = 1024;
                screenHeight = 768;
                break;
            case 9:
                screenWidth = 800;
                screenHeight = 600;
                break;
            case 10:
                screenWidth = 640;
                screenHeight = 480;
                break;
            default:
                break;
        }

        Screen.SetResolution(screenWidth, screenHeight, Screen.fullScreenMode);
    }

    public void SetWindowMode()
    {
        switch (windowMode.value)
        {
            case 0:
                Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                break;
            case 1:
                Screen.fullScreenMode = FullScreenMode.Windowed;
                break;
            case 2:
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                break;
            default:
                break;
        }
    }
    #endregion


}
