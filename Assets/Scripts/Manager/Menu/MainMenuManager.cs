using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Manager.Menu
{
    public class MainMenuManager : MonoBehaviour
    {
        private const int GameScene = 1;
        private const float startTime = 1f;

        private void Awake()
        {
            Time.timeScale = startTime;
        }

        public void OnStartClick()
        {
            StartCoroutine(nameof(StartGame));
        }

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
