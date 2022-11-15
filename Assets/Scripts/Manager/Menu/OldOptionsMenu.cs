using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.Audio;
using UnityEngine.UI;
using Cameras;

public class OldOptionsMenu : MonoBehaviour
{
    //Graphic settings
    [SerializeField] private TMP_Dropdown resolution;
    [SerializeField] private TextMeshProUGUI resolutionText;
    [SerializeField] private TMP_Dropdown windowMode;
    [SerializeField] private TextMeshProUGUI windowModeText;

    //Audio settings
    [SerializeField] private AudioMixer masterVolume;
    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;

    //Control settings
    [SerializeField] private CameraControllerNew cameraSensitivity;
    [SerializeField] private Slider cameraSensitivitySlider;
    [SerializeField] private Toggle invertedControlToggle;

    private int screenWidth = 1920;
    private int screenHeight = 1080;
    
    //Input
    [SerializeField] private PlayerInput playerInput;
    private bool playerInputHasBeenInit;

    private void Awake()
    {
        Screen.fullScreen = true;
    }

    private void Start()
    {
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        resolutionText.text = $"{screenWidth}x{screenHeight}";
        switch(resolutionText.text)
        {
            case "2560x1440":
                resolution.value = 0;
                break;
            case "1920x1200":
                resolution.value = 1;
                break;
            case "1920x1080":
                resolution.value = 2;
                break;
            case "1680x1050":
                resolution.value = 3;
                break;
            case "1440x900":
                resolution.value = 4;
                break;
            case "1366x768":
                resolution.value = 5;
                break;
            case "1280x800":
                resolution.value = 6;
                break;
            case "1280x720":
                resolution.value = 7;
                break;
            case "1024x768":
                resolution.value = 8;
                break;
            case "800x600":
                resolution.value = 9;
                break;
            case "640x480":
                resolution.value = 10;
                break;
            default:
                break;
        }

        switch (Screen.fullScreenMode)
        {
            case FullScreenMode.ExclusiveFullScreen:
                windowModeText.text = "Vollbild";
                windowMode.value = 0;
                break;
            case FullScreenMode.Windowed:
                windowModeText.text = "Fenster";
                windowMode.value = 1;
                break;
            case FullScreenMode.FullScreenWindow:
                windowModeText.text = "Rahmenloses Fenster";
                windowMode.value = 2;
                break;
            default:
                break;
        }

        cameraSensitivitySlider.value = cameraSensitivity.RotationSensivity;
    }

    private void Update()
    {
        if (!playerInputHasBeenInit)
        {
            InitPlayerInput();
        }
    }

    private void OnDisable()
    {
        playerInput.actions["OpenEscapeMenu"].performed -= EnableDisableMenu;
        playerInputHasBeenInit = false;
    }
    
    private void InitPlayerInput()
    {
        playerInput.actions["OpenEscapeMenu"].performed += EnableDisableMenu;
        playerInputHasBeenInit = true;
    }

    private void EnableDisableMenu(InputAction.CallbackContext context)
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

    #region Audio options
    public void OnMasterVolumeChanged(float sliderValue)
    {
        masterVolume.SetFloat("exposedMasterVolume", Mathf.Log10(sliderValue) * 20);
    }

    public void OnMusicVolumeChanged(float sliderValue)
    {
        masterVolume.SetFloat("exposedMusicVolume", Mathf.Log10(sliderValue) * 20);
    }

    public void OnSFXVolumeChanged(float sliderValue)
    {
        masterVolume.SetFloat("exposedSFXVolume", Mathf.Log10(sliderValue) * 20);
    }
    #endregion

    #region Control options
    public void OnControlSensitivityChanged()
    {
        cameraSensitivity.RotationSensivity = cameraSensitivitySlider.value;
        cameraSensitivity.MoveSensivity = cameraSensitivitySlider.value;
    }
    #endregion
}
