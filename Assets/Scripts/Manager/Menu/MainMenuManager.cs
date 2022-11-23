using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Manager.Menu
{
    public class MainMenuManager : MonoBehaviour //Made by Eric
    {
        private const int GameScene = 1;
        private const float startTime = 1f;

        private void Awake()
        {
            Time.timeScale = startTime;
        }

        /// <summary>
        /// Method for start button to load the game scene
        /// </summary>
        public void OnStartClick()
        {
            StartCoroutine(nameof(StartGame));
        }

        /// <summary>
        /// Method for the quit button to stop the application
        /// </summary>
        public void OnExitGameClick()
        {
            Application.Quit();
        }

        IEnumerator StartGame()
        {
            yield return new WaitForSeconds(1f);
            SceneManager.LoadScene(GameScene);
        }
    }
}
