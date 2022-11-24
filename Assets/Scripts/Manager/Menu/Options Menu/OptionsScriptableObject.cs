using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new option", menuName = "Options")]
public class OptionsScriptableObject : ScriptableObject
{
    //Graphic Settings
    [SerializeField] private int resolutionCase;
    public int ResolutionCase
    {
        get => resolutionCase;
        set { resolutionCase = value; }
    }

    private int optionsScreenWidth;
    public int OptionsScreenWidth
    {
        get => optionsScreenWidth;
        set { optionsScreenWidth = value; }
    }

    private int optionsScreenHeight;
    public int OptionsScreenHeight
    {
        get => optionsScreenHeight;
        set { optionsScreenHeight = value; }
    }

    [SerializeField] private int windowModeCase;
    public int WindowModeCase
    {
        get => windowModeCase;
        set { windowModeCase = value; }
    }

    //Audio settings
    [SerializeField, Range(0f, 1f)] private float masterVolumeSetting;
    public float MasterVolumeSetting
    {
        get => masterVolumeSetting;
        set { masterVolumeSetting = value; }
    }

    [SerializeField, Range(0f, 1f)] private float musicVolumeSetting;
    public float MusicVolumeSetting
    {
        get => musicVolumeSetting;
        set { musicVolumeSetting = value; }
    }

    [SerializeField, Range(0f, 1f)] private float sfxVolumeSetting;
    public float SFXVolumeSetting
    {
        get => sfxVolumeSetting;
        set { sfxVolumeSetting = value; }
    }

    //Control settings
    [SerializeField, Range(0.5f, 10f)] private float cameraSensitivitySetting;
    public float CameraSensitivitySetting
    {
        get => cameraSensitivitySetting;
        set { cameraSensitivitySetting = value; }
    }

    [SerializeField] private bool invertedIsActive;
    public bool InvertedIsActive
    {
        get => invertedIsActive;
        set { invertedIsActive = value; }
    }
}
