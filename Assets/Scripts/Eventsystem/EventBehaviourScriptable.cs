using Manager;
using ResourceManagement;
using ResourceManagement.Manager;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Eventsystem
{
    public class EventBehaviourScriptable : MonoBehaviour //Made by Eric
    {
        #region Variables
        
        //EventBehaviour related variables
        private EventManagerScriptable eventManager;
        private ResourceManager multiResourceManager;
        private bool textIsActive;
        [SerializeField] private GameObject eventUI;
        [SerializeField] private TextMeshProUGUI eventTitle;
        [SerializeField] private TextMeshProUGUI eventText;
        [SerializeField] private GameObject decision1Button;
        [SerializeField] private GameObject decision2Button;
        [SerializeField] private TextMeshProUGUI decision1ButtonText;
        [SerializeField] private TextMeshProUGUI decision2ButtonText;
        private int textIndex;
        private const int ResetTextIndex = 0;
        private bool endOfEvent;
        private const int Decision1Index = 0;
        private const int Decision2Index = 1;
        private bool buttonIsActive;

        //Text animation related variables used mostly in Update()
        private float charTitlePlacingTime;
        private float charTextPlacingTime;
        private const float ResetTitlePlacingTime = 0.1f;
        private const float ResetTextPlacingTime = 0.03f;
        private const float EndPlacingTime = 0f;
        private int charTitleIndex;
        private int charTextIndex;
        private bool decision1Active;
        private bool decision2Active;
        private int charDecision1Index;
        private int charDecision2Index;

        //Input
        [SerializeField] private PlayerInput playerInput;
        private bool playerInputHasBeenInit;
        
        #endregion

        #region UnityEvents
        
        // private void Start()
        // {
        //     eventManager = MainManagerSingleton.Instance.EventManager;
        //     eventUI.SetActive(false);
        //     textIsActive = true;
        //     textIndex = resetTextIndex;
        //     enabled = false;
        // }

        private void OnEnable()
        {
            textIsActive = true;
            textIndex = ResetTextIndex;

            eventUI.SetActive(true);
            
            decision1Active = false;
            decision2Active = false;

            charDecision1Index = 0;
            charDecision2Index = 0;

            eventTitle.text = "";
            eventText.text = "";

            charTitleIndex = ResetTextIndex;
            charTextIndex = ResetTextIndex;
        }

        private void Update()
        {
            
            if (!playerInputHasBeenInit)
            {
                InitPlayerInput();
            }
            
            charTitlePlacingTime -= Time.unscaledDeltaTime;
            charTextPlacingTime -= Time.unscaledDeltaTime;

            //Fades in one char after another for the title
            if (eventTitle.text.Length < eventManager.ActiveEvent.EventTitle.Length && charTitlePlacingTime <= EndPlacingTime)
            {
                eventTitle.text += eventManager.ActiveEvent.EventTitle[charTitleIndex];
                charTitleIndex++;
                charTitlePlacingTime = ResetTitlePlacingTime;
            }

            //Fades in one char after another for each event texts of the active event
            if (eventText.text.Length < eventManager.ActiveEvent.EventText[textIndex].Length && charTextPlacingTime <= EndPlacingTime && !decision1Active && !decision2Active)
            {
                eventText.text += eventManager.ActiveEvent.EventText[textIndex][charTextIndex];
                charTextIndex++;
                charTextPlacingTime = ResetTextPlacingTime;
            }

            //Fades in one char after another for each decision text of the first decision
            if (eventText.text.Length < eventManager.ActiveEvent.Decisions[Decision1Index].consequenceText.Length && charTextPlacingTime <= EndPlacingTime && decision1Active)
            {
                eventText.text += eventManager.ActiveEvent.Decisions[Decision1Index].consequenceText[charDecision1Index];
                charDecision1Index++;
                charTextPlacingTime = ResetTextPlacingTime;
            }

            //Fades in one char after another for each decision text of the second decision
            if (eventText.text.Length < eventManager.ActiveEvent.Decisions[Decision2Index].consequenceText.Length && charTextPlacingTime <= EndPlacingTime && decision2Active)
            {
                eventText.text += eventManager.ActiveEvent.Decisions[Decision2Index].consequenceText[charDecision2Index];
                charDecision2Index++;
                charTextPlacingTime = ResetTextPlacingTime;
            }
        }
        
        private void OnDisable()
        {
            playerInput.actions["LeftClick"].performed -= EventBehaviour;
            eventManager.ResetEventTimer?.Invoke();                         //calls an Action in EventManagerScriptable to reset and start the event timer there
            playerInputHasBeenInit = false;
        }

        #endregion

        #region Methods

         public void Initialize()
         {
            eventManager = MainManagerSingleton.Instance.EventManager;
            eventUI.SetActive(false);
            textIsActive = true;
            textIndex = ResetTextIndex;
            // enabled = false;
         }
        
        private void InitPlayerInput()
        {
            playerInput.actions["LeftClick"].performed += EventBehaviour;
            playerInputHasBeenInit = true;
        }
        
        /// <summary>
        /// Method to be called in Player Input Component, must be new script in order to only use it when an event was chosen and activated
        /// </summary>
        private void EventBehaviour(InputAction.CallbackContext context)
        {
            if (!context.performed || enabled != true) return;

            //Instantly places current text when clicking again after switching to the next text
            if (eventText.text.Length < eventManager.ActiveEvent.EventText[textIndex].Length && textIndex < eventManager.ActiveEvent.EventText.Length && !decision1Active && !decision2Active)
            {
                eventText.text = eventManager.ActiveEvent.EventText[textIndex];
                return;
            }

            //Instantly places text for decision one after switching to it
            if (eventText.text.Length < eventManager.ActiveEvent.Decisions[Decision1Index].consequenceText.Length && decision1Active)
            {
                eventText.text = eventManager.ActiveEvent.Decisions[Decision1Index].consequenceText;
                return;
            }

            //Instantly places text for decision two after switching to it
            if (eventText.text.Length < eventManager.ActiveEvent.Decisions[Decision2Index].consequenceText.Length && decision2Active)
            {
                eventText.text = eventManager.ActiveEvent.Decisions[Decision2Index].consequenceText;
                return;
            }

            //Select next text of each text array
            if (textIsActive && textIndex < eventManager.ActiveEvent.EventText.Length)
            {
                eventText.text = "";
                charTextIndex = ResetTextIndex;
                textIndex++;
                buttonIsActive = true;
            }

            //Activation of the decision buttons and disabling the ability to continue the event texts by clicking
            if (buttonIsActive && textIndex >= eventManager.ActiveEvent.EventText.Length - 1)
            {
                decision1Button.SetActive(true);
                decision2Button.SetActive(true);
                decision1ButtonText.text = eventManager.ActiveEvent.Decisions[Decision1Index].decisionButtonText;
                decision2ButtonText.text = eventManager.ActiveEvent.Decisions[Decision2Index].decisionButtonText;
                textIsActive = false;
                buttonIsActive = false;
            }

            //Ends the event by disabling the script, thus triggering the end logic in OnDisable()
            if (!endOfEvent) return;
            
            eventUI.SetActive(false);
            endOfEvent = false;
            enabled = false;
        }

        /// <summary>
        /// Transfers all information of the scriptable object to the UI elements and adds or removes resources based on those information
        /// </summary>
        public void Decision1()
        {
            decision1Button.SetActive(false);
            decision2Button.SetActive(false);
            decision1Active = true;
            eventText.text = "";
            for (int i = 0; i < eventManager.ActiveEvent.Decisions[Decision1Index].consequenceOnResources.Length; i++)
            {
                switch (eventManager.ActiveEvent.Decisions[Decision1Index].consequenceOnResources[i].resource)
                {
                    case ResourceType.Material:
                        multiResourceManager = MainManagerSingleton.Instance.GetComponent<MaterialManager>();
                        break;
                    case ResourceType.Energy:
                        multiResourceManager = MainManagerSingleton.Instance.GetComponent<EnergyManager>();
                        break;
                    case ResourceType.Citizen:
                        multiResourceManager = MainManagerSingleton.Instance.GetComponent<CitizenManager>();
                        break;
                    case ResourceType.Food:
                        multiResourceManager = MainManagerSingleton.Instance.GetComponent<FoodManager>();
                        break;
                    case ResourceType.Water:
                        multiResourceManager = MainManagerSingleton.Instance.GetComponent<WaterManager>();
                        break;
                }

                multiResourceManager.SavedResourceValue = Mathf.Clamp(multiResourceManager.SavedResourceValue += multiResourceManager.SaveSpace * eventManager.ActiveEvent.Decisions[Decision1Index].consequenceOnResources[i].eventResourceDemandValue, 0f, multiResourceManager.SaveSpace);
            }
            endOfEvent = true;
        }

        /// <summary>
        /// Transfers all information of the scriptable object to the UI elements and adds or removes resources based on those information
        /// </summary>
        public void Decision2()
        {
            decision1Button.SetActive(false);
            decision2Button.SetActive(false);
            decision2Active = true;
            eventText.text = "";
            for (int i = 0; i < eventManager.ActiveEvent.Decisions[Decision2Index].consequenceOnResources.Length; i++)
            {
                switch (eventManager.ActiveEvent.Decisions[Decision2Index].consequenceOnResources[i].resource)
                {
                    case ResourceType.Material:
                        multiResourceManager = MainManagerSingleton.Instance.GetComponent<MaterialManager>();
                        break;
                    case ResourceType.Energy:
                        multiResourceManager = MainManagerSingleton.Instance.GetComponent<EnergyManager>();
                        break;
                    case ResourceType.Citizen:
                        multiResourceManager = MainManagerSingleton.Instance.GetComponent<CitizenManager>();
                        break;
                    case ResourceType.Food:
                        multiResourceManager = MainManagerSingleton.Instance.GetComponent<FoodManager>();
                        break;
                    case ResourceType.Water:
                        multiResourceManager = MainManagerSingleton.Instance.GetComponent<WaterManager>();
                        break;
                }

                multiResourceManager.SavedResourceValue = Mathf.Clamp(multiResourceManager.SavedResourceValue += multiResourceManager.SaveSpace * eventManager.ActiveEvent.Decisions[Decision2Index].consequenceOnResources[i].eventResourceDemandValue, 0f, multiResourceManager.SaveSpace);
            }
            endOfEvent = true;
        }

        #endregion
    }
}
