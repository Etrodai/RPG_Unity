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
    
    private bool isOpen;

    private void Awake()
    {
        if (!Instance) Instance = this;
        else
        {
            Destroy(gameObject);
            throw new System.Exception("There are two DebugMenus in the scene!");
        }
        
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        menu.SetActive(false);
    }

    private void Update()
    {
        //                                                              TODO => use new InputSystem
        // if (Input.GetKeyDown(KeyCode.Delete)) ToggleMenu();
        // if (Input.GetKeyDown(KeyCode.Tab) && isOpen) cheatConsole.EnablePanel();
    }

    private void ToggleMenu()
    {
        isOpen = !isOpen;
        menu.SetActive(isOpen);
        cheatConsole.DisablePanel();
        // if (isOpen) GameManager.Instance.DisablePlayerMovement();
        // else GameManager.Instance.EnablePlayerMovement();
    }
}
