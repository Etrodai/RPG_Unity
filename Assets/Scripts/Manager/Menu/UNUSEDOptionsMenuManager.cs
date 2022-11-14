using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UNUSEDOptionsMenuManager : MonoBehaviour
{
    private UIDocument optionsDocument;
    private DropdownField resolutionOption;
    public DropdownField ResolutionOption => resolutionOption;
    private DropdownField windowModeOption;
    private List<string> resolutions = new List<string>();
    private List<string> windowModes = new List<string>();

    private int screenWidth;
    private int screenHeight;

    private void Awake()
    {
        //FillResolutionsList();
        //FillWindowModeList();
    }

    private void OnEnable()
    {
        //optionsDocument = GetComponent<UIDocument>();
        //resolutionOption = optionsDocument.rootVisualElement.Q<DropdownField>("options-resolution-dropdown");
        //windowModeOption = optionsDocument.rootVisualElement.Q<DropdownField>("options-screen-type-dropdown");

        //resolutionOption.choices = resolutions;
        //windowModeOption.choices = windowModes;

        //resolutionOption.RegisterValueChangedCallback(trigger => SetResolution());
        //windowModeOption.RegisterValueChangedCallback(trigger => SetWindowMode());
    }

    private void Start()
    {
        gameObject.SetActive(false);
    }

    //private void SetResolution()
    //{
    //    switch (resolutionOption.index)
    //    {
    //        case 0:
    //            screenWidth = 2560;
    //            screenHeight = 1440;
    //            break;
    //        case 1:
    //            screenWidth = 1920;
    //            screenHeight = 1200;
    //            break;
    //        case 2:
    //            screenWidth = 1920;
    //            screenHeight = 1080;
    //            break;
    //        case 3:
    //            screenWidth = 1680;
    //            screenHeight = 1050;
    //            break;
    //        case 4:
    //            screenWidth = 1440;
    //            screenHeight = 900;
    //            break;
    //        case 5:
    //            screenWidth = 1366;
    //            screenHeight = 768;
    //            break;
    //        case 6:
    //            screenWidth = 1280;
    //            screenHeight = 800;
    //            break;
    //        case 7:
    //            screenWidth = 1280;
    //            screenHeight = 720;
    //            break;
    //        case 8:
    //            screenWidth = 1024;
    //            screenHeight = 768;
    //            break;
    //        case 9:
    //            screenWidth = 800;
    //            screenHeight = 600;
    //            break;
    //        case 10:
    //            screenWidth = 640;
    //            screenHeight = 480;
    //            break;
    //        default:
    //            break;
    //    }

    //    Screen.SetResolution(screenWidth, screenHeight, Screen.fullScreenMode);
    //}

    //private void SetWindowMode()
    //{
    //    switch (windowModeOption.index)
    //    {
    //        case 0:
    //            Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
    //            break;
    //        case 1:
    //            Screen.fullScreenMode = FullScreenMode.Windowed;
    //            break;
    //        case 2:
    //            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
    //            break;
    //        default:
    //            break;
    //    }
    //}

    //private void FillResolutionsList()
    //{
    //    resolutions.Add("2560x1440");
    //    resolutions.Add("1920x1200");
    //    resolutions.Add("1920x1080");
    //    resolutions.Add("1680x1050");
    //    resolutions.Add("1440x900");
    //    resolutions.Add("1366x768");
    //    resolutions.Add("1280x800");
    //    resolutions.Add("1280x720");
    //    resolutions.Add("1024x768");
    //    resolutions.Add("800x600");
    //    resolutions.Add("640x480");
    //}

    //private void FillWindowModeList()
    //{
    //    windowModes.Add("Vollbild");
    //    windowModes.Add("Fenster");
    //    windowModes.Add("Rahmenloses Fenster");
    //}
}
