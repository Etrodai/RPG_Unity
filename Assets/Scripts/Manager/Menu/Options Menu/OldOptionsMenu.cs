using Cameras;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Manager.Menu
{
    public class OldOptionsMenu : MonoBehaviour //Made by Eric
    {
        //ScriptableObject save
        [SerializeField] private OptionsScriptableObject optionsScriptable;

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

        //private int screenWidth;
        //private int screenHeight;
    
        //Input
        [SerializeField] private PlayerInput playerInput;
        private bool playerInputHasBeenInit;
        
        #region Unity Methods
        private void Awake()
        {
            Screen.fullScreen = true;
        }

        /// <summary>
        /// Sets all options to their set value
        /// </summary>
        private void Start()
        {
            resolution.value = optionsScriptable.ResolutionCase;
            SetResolution();
            windowMode.value = optionsScriptable.WindowModeCase;
            SetWindowMode();

            masterVolume.SetFloat("exposedMasterVolume", Mathf.Log10(optionsScriptable.MasterVolumeSetting) * 20);
            masterVolume.SetFloat("exposedMusicVolume", Mathf.Log10(optionsScriptable.MusicVolumeSetting) * 20);
            masterVolume.SetFloat("exposedSFXVolume", Mathf.Log10(optionsScriptable.SFXVolumeSetting) * 20);

            if (SceneManager.GetActiveScene().buildIndex != 0)
            {
                cameraSensitivity.RotationSensitivity = optionsScriptable.CameraSensitivitySetting; 
                cameraSensitivity.MoveSensitivity = optionsScriptable.CameraSensitivitySetting;
                cameraSensitivity.InvertYAxis(optionsScriptable.InvertedIsActive);

            }
            gameObject.SetActive(false);
        }
        
        /// <summary>
        /// Sets the texts and values of the option ui to the set values
        /// </summary>
        private void OnEnable()
        {
            if (SceneManager.GetActiveScene().buildIndex != 0)
            {
                resolutionText.text = $"{optionsScriptable.OptionsScreenWidth}x{optionsScriptable.OptionsScreenHeight}";
                switch (optionsScriptable.ResolutionCase)
                {
                    case 0:
                        resolution.value = 0;
                        break;
                    case 1:
                        resolution.value = 1;
                        break;
                    case 2:
                        resolution.value = 2;
                        break;
                    case 3:
                        resolution.value = 3;
                        break;
                    case 4:
                        resolution.value = 4;
                        break;
                    case 5:
                        resolution.value = 5;
                        break;
                    case 6:
                        resolution.value = 6;
                        break;
                    case 7:
                        resolution.value = 7;
                        break;
                    case 8:
                        resolution.value = 8;
                        break;
                    case 9:
                        resolution.value = 9;
                        break;
                    case 10:
                        resolution.value = 10;
                        break;
                }

                switch (optionsScriptable.WindowModeCase)
                {
                    case 0:
                        windowModeText.text = "Vollbild";
                        windowMode.value = 0;
                        break;
                    case 1:
                        windowModeText.text = "Fenster";
                        windowMode.value = 1;
                        break;
                    case 2:
                        windowModeText.text = "Rahmenloses Fenster";
                        windowMode.value = 2;
                        break;
                }

                masterVolumeSlider.value = optionsScriptable.MasterVolumeSetting;
                musicVolumeSlider.value = optionsScriptable.MusicVolumeSetting;
                sfxVolumeSlider.value = optionsScriptable.SFXVolumeSetting;

                cameraSensitivitySlider.value = optionsScriptable.CameraSensitivitySetting;
                invertedControlToggle.isOn = optionsScriptable.InvertedIsActive;
            }

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
            if (SceneManager.GetActiveScene().buildIndex != 0)
            {
                playerInput.actions["OpenEscapeMenu"].performed -= DisableMenu;
                playerInputHasBeenInit = false; 
            }
        }
        #endregion

        private void InitPlayerInput()
        {
            if (SceneManager.GetActiveScene().buildIndex != 0)
            {
                playerInput.actions["OpenEscapeMenu"].performed += DisableMenu;
                playerInputHasBeenInit = true; 
            }
        }

        public void DisableMenu(InputAction.CallbackContext context)
        {
            if (gameObject.activeInHierarchy && context.performed)
            {
                gameObject.SetActive(false);
            }
        }

        #region Graphic options
        /// <summary>
        /// Sets the resolution to chosen setting
        /// </summary>
        public void SetResolution()
        {
            switch (resolution.value)
            {
                case 0:
                    optionsScriptable.ResolutionCase = 0;
                    optionsScriptable.OptionsScreenWidth = 2560;
                    optionsScriptable.OptionsScreenHeight = 1440;
                    break;
                case 1:
                    optionsScriptable.ResolutionCase = 1;
                    optionsScriptable.OptionsScreenWidth = 1920;
                    optionsScriptable.OptionsScreenHeight = 1200;
                    break;
                case 2:
                    optionsScriptable.ResolutionCase = 2;
                    optionsScriptable.OptionsScreenWidth = 1920;
                    optionsScriptable.OptionsScreenHeight = 1080;
                    break;
                case 3:
                    optionsScriptable.ResolutionCase = 3;
                    optionsScriptable.OptionsScreenWidth = 1680;
                    optionsScriptable.OptionsScreenHeight = 1050;
                    break;
                case 4:
                    optionsScriptable.ResolutionCase = 4;
                    optionsScriptable.OptionsScreenWidth = 1440;
                    optionsScriptable.OptionsScreenHeight = 900;
                    break;
                case 5:
                    optionsScriptable.ResolutionCase = 5;
                    optionsScriptable.OptionsScreenWidth = 1366;
                    optionsScriptable.OptionsScreenHeight = 768;
                    break;
                case 6:
                    optionsScriptable.ResolutionCase = 6;
                    optionsScriptable.OptionsScreenWidth = 1280;
                    optionsScriptable.OptionsScreenHeight = 800;
                    break;
                case 7:
                    optionsScriptable.ResolutionCase = 7;
                    optionsScriptable.OptionsScreenWidth = 1280;
                    optionsScriptable.OptionsScreenHeight = 720;
                    break;
                case 8:
                    optionsScriptable.ResolutionCase = 8;
                    optionsScriptable.OptionsScreenWidth = 1024;
                    optionsScriptable.OptionsScreenHeight = 768;
                    break;
                case 9:
                    optionsScriptable.ResolutionCase = 9;
                    optionsScriptable.OptionsScreenWidth = 800;
                    optionsScriptable.OptionsScreenHeight = 600;
                    break;
                case 10:
                    optionsScriptable.ResolutionCase = 10;
                    optionsScriptable.OptionsScreenWidth = 640;
                    optionsScriptable.OptionsScreenHeight = 480;
                    break;
            }

            Screen.SetResolution(optionsScriptable.OptionsScreenWidth, optionsScriptable.OptionsScreenHeight, Screen.fullScreenMode);
        }

        /// <summary>
        /// Sets the window mode to the chosen value
        /// </summary>
        public void SetWindowMode()
        {
            switch (windowMode.value)
            {
                case 0:
                    optionsScriptable.WindowModeCase = 0;
                    Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                    break;
                case 1:
                    optionsScriptable.WindowModeCase = 1;
                    Screen.fullScreenMode = FullScreenMode.Windowed;
                    break;
                case 2:
                    optionsScriptable.WindowModeCase = 2;
                    Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                    break;
            }
        }
        #endregion

        #region Audio options
        /// <summary>
        /// Master volume
        /// </summary>
        /// <param name="sliderValue"></param>
        public void OnMasterVolumeChanged(float sliderValue)
        {
            optionsScriptable.MasterVolumeSetting = sliderValue;
            masterVolume.SetFloat("exposedMasterVolume", Mathf.Log10(sliderValue) * 20);
        }

        /// <summary>
        /// Music volume
        /// </summary>
        /// <param name="sliderValue"></param>
        public void OnMusicVolumeChanged(float sliderValue)
        {
            optionsScriptable.MusicVolumeSetting = sliderValue;
            masterVolume.SetFloat("exposedMusicVolume", Mathf.Log10(sliderValue) * 20);
        }

        /// <summary>
        /// SFX volume
        /// </summary>
        /// <param name="sliderValue"></param>
        public void OnSFXVolumeChanged(float sliderValue)
        {
            optionsScriptable.SFXVolumeSetting = sliderValue;
            masterVolume.SetFloat("exposedSFXVolume", Mathf.Log10(sliderValue) * 20);
        }
        #endregion

        #region Control options
        /// <summary>
        /// Sets the sensitivity of the camera movement
        /// </summary>
        public void OnControlSensitivityChanged()
        {
            optionsScriptable.CameraSensitivitySetting = cameraSensitivitySlider.value;
            cameraSensitivity.RotationSensitivity = optionsScriptable.CameraSensitivitySetting;
            cameraSensitivity.MoveSensitivity = optionsScriptable.CameraSensitivitySetting;
        }

        /// <summary>
        /// Inverts inputs for up and down
        /// </summary>
        public void InvertCameraControls()
        {
            if (optionsScriptable.InvertedIsActive)
            {
                optionsScriptable.InvertedIsActive = false;
            }
            else
            {
                optionsScriptable.InvertedIsActive = true;
            }
            cameraSensitivity.InvertYAxis(optionsScriptable.InvertedIsActive);
        }
        #endregion
    }
}
