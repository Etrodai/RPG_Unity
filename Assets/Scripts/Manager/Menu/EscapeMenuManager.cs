using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace Manager.Menu
{
    public class EscapeMenuManager : MonoBehaviour
    {
        //[SerializeField] private UIDocument escapeDocument;
        [SerializeField] private GameObject escapeMenu;
        [SerializeField] private GameObject optionsMenu;

        private const int mainMenuScene = 0;

        private void Start()
        {
            //    escapeDocument.rootVisualElement.Q<Button>("escape-resume-button").clickable.clicked += OnResumeClick;
            //    escapeDocument.rootVisualElement.Q<Button>("escape-options-button").clickable.clicked += OnOptionsClick;

            escapeMenu.SetActive(false);
        }

        public void EnableDisableMenu()
        {
            if(escapeMenu.activeInHierarchy == false)
            {
                Time.timeScale = 0f;
                escapeMenu.SetActive(true);
            }
            else
            {
                Time.timeScale = 1f;
                escapeMenu.SetActive(false);
            }
        }

        public void OnResumeClick()
        {
            Time.timeScale = 1f;
            escapeMenu.SetActive(false);
        }

        public void OnOptionsClick()
        {
            escapeMenu.SetActive(false);
            optionsMenu.SetActive(true);
        }

        public void OnBackToEscapeMenuClick()
        {
            optionsMenu.SetActive(false);
            escapeMenu.SetActive(true);
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
