using System;
using UnityEngine;
using UnityEngine.InputSystem;
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
        
        //Input
        [SerializeField] private PlayerInput playerInput;
        private bool playerInputHasBeenInit;

        private void Start()
        {
            //    escapeDocument.rootVisualElement.Q<Button>("escape-resume-button").clickable.clicked += OnResumeClick;
            //    escapeDocument.rootVisualElement.Q<Button>("escape-options-button").clickable.clicked += OnOptionsClick;

            escapeMenu.SetActive(false);
        }

        private void Update()
        {
            if (!playerInputHasBeenInit)
            {
                InitPlayerInput();
            }
        }

        private void OnDisable()
        {
            if (playerInput == null) return;
            playerInput.actions["OpenEscapeMenu"].performed -= EnableDisableMenu;
        }
        
        private void InitPlayerInput()
        {
            playerInput.actions["OpenEscapeMenu"].performed += EnableDisableMenu;
        }
        public void EnableDisableMenu(InputAction.CallbackContext context)
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
