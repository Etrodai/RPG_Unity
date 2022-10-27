using UnityEngine;
using UnityEngine.SceneManagement;

namespace Manager.Menu
{
    public class EscapeMenuManager : MonoBehaviour
    {
        [SerializeField]
        private GameObject placeHolder;

        private const int mainMenuScene = 0;

        public void EnableDisableMenu()
        {
            if(placeHolder.activeInHierarchy == false)
            {
                Time.timeScale = 0f;
                placeHolder.SetActive(true);
            }
            else
            {
                Time.timeScale = 1f;
                placeHolder.SetActive(false);
            }
        }

        public void OnResumeClick()
        {
            Time.timeScale = 1f;
            placeHolder.SetActive(false);
        }

        public void OnMainMenuClick()
        {
            //Probably place where saving must take place
            SceneManager.LoadScene(mainMenuScene);
        }

        public void OnExitGameClick()
        {
            //Saving needed here as well
            Application.Quit();
        }
    }
}
