using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Manager.Menu
{
    public class EscapeMenuManager : MonoBehaviour //Made by Eric
    {
        //[SerializeField] private UIDocument escapeDocument;
        [SerializeField] private GameObject escapeMenu;
        [SerializeField] private GameObject optionsMenu;

        private const int MainMenuScene = 0;
        
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
            playerInputHasBeenInit = false;
        }
        
        private void InitPlayerInput()
        {
            playerInput.actions["OpenEscapeMenu"].performed += EnableDisableMenu;
            playerInputHasBeenInit = true;
        }

        /// <summary>
        /// Switches the Escape menu UI on or off depending on its current state
        /// </summary>
        /// <param name="context">Register the esc input</param>
        private void EnableDisableMenu(InputAction.CallbackContext context)
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
        
        public void EnableMenuOnClick()
        {
            if(escapeMenu.activeInHierarchy == false)
            {
                Time.timeScale = 0f;
                escapeMenu.SetActive(true);
            }
        }

        /// <summary>
        /// Method for the resume button to get back into the game
        /// </summary>
        public void OnResumeClick()
        {
            Time.timeScale = 1f;
            escapeMenu.SetActive(false);
        }

        /// <summary>
        /// Method for the options button to activate the options menu ui
        /// </summary>
        public void OnOptionsClick()
        {
            escapeMenu.SetActive(false);
            optionsMenu.SetActive(true);
        }

        /// <summary>
        /// Method for returning back into the escape menu from options menu
        /// </summary>
        public void OnBackToEscapeMenu()
        {
            optionsMenu.SetActive(false);
            escapeMenu.SetActive(true);
        }

        /// <summary>
        /// Method for exit button to load the main menu scene
        /// </summary>
        public void OnMainMenuClick()
        {
            //Probably place where saving must take place
            //is now assigned in Inspector
            SceneManager.LoadScene(MainMenuScene);
        }
    }
}
