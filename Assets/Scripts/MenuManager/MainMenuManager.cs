using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    private const int gameScene = 1;

    public void OnStartClick()
    {
        SceneManager.LoadScene(gameScene);
    }

    public void OnExitGameClick()
    {
        Application.Quit();
    }
}
