using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugMenu : MonoBehaviour
{
    public static DebugMenu Instance { get; private set; }
    
    [SerializeField] private GameObject menu;
    [SerializeField] private CheatConsole cheatConsole;
    
    public bool isOpen;

    private void Awake()
    {
        if (!Instance) Instance = this;
        else
        {
            Destroy(gameObject);
            throw new System.Exception("There are two DebugMenus in the scene!");
        }
    }

    private void Start()
    {
        menu.SetActive(false);
    }

    public void ToggleMenu()
    {
        isOpen = !isOpen;
        menu.SetActive(isOpen);
        cheatConsole.DisablePanel();
        Time.timeScale = isOpen ? 0 : 1;
    }
}
