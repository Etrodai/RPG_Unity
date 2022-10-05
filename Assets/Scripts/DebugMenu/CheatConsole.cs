using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CheatConsole : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private GameObject cheatConsolePanel;
    [SerializeField] private CheatSheet cheatSheet;

    private void Start()
    {
        cheatConsolePanel.SetActive(false);
    }

    public void EnablePanel()
    {
        if (DebugMenu.Instance.isOpen)
        {
            cheatConsolePanel.SetActive(true);
            inputField.Select();
        }
    }

    public void DisablePanel() => cheatConsolePanel.SetActive(false);

    public void ProcessCheatInput()
    {
        cheatSheet.ProcessInput(inputField.text);
    }
}
