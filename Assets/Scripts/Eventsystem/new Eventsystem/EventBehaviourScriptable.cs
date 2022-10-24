using ResourceManagement;
using ResourceManagement.Manager;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EventBehaviourScriptable : MonoBehaviour
{
    //EventBehaviour related variables
    private EventmanagerScriptable eventManager;
    private MonoBehaviour multiResourceManager;
    private bool textIsActive;
    [SerializeField] private GameObject eventUI;
    [SerializeField] private TextMeshProUGUI eventTitle;
    [SerializeField] private TextMeshProUGUI eventText;
    [SerializeField] private GameObject decision1Button;
    [SerializeField] private GameObject decision2Button;
    [SerializeField] private TextMeshProUGUI decision1ButtonText;
    [SerializeField] private TextMeshProUGUI decision2ButtonText;
    private int textIndex;
    private const int resetTextIndex = 0;
    private bool endofEvent;
    private const int decision1Index = 0;
    private const int decision2Index = 1;
    private bool buttonIsActive;

    private void Start()
    {
        eventManager = MainManagerSingleton.Instance.GetComponent<EventmanagerScriptable>();
        eventUI.SetActive(false);
        textIsActive = true;
        textIndex = resetTextIndex;
        enabled = false;
    }

    private void OnDisable()
    {
        eventManager.ResetEventTimer?.Invoke();                         //calls an Action in EventmanagerScriptable to reset and start the event timer there
    }

    private void OnEnable()
    {
        textIsActive = true;
        textIndex = resetTextIndex;

        eventUI.SetActive(true);

        if (eventManager.ActiveEvent != null)
        {
            eventTitle.text = eventManager.ActiveEvent.EventTitle;  
        }

        EventBehaviour();                                               //calls EventBehaviour to start the interactive text
    }

    public void EventBehaviour() //Method to be called in Player Input Component, must be new script in order to only use it when an event was chosen and activated
    {
        //Texts for duration of length of each text array
        if (textIsActive && textIndex < eventManager.ActiveEvent.EventText.Length)
        {
            eventText.text = eventManager.ActiveEvent.EventText[textIndex];
            textIndex++;
            buttonIsActive = true;
        }

        if (buttonIsActive && textIndex >= eventManager.ActiveEvent.EventText.Length)
        {
            decision1Button.SetActive(true);
            decision2Button.SetActive(true);
            decision1ButtonText.text = eventManager.ActiveEvent.Decisions[decision1Index].decisionButtonText;
            decision2ButtonText.text = eventManager.ActiveEvent.Decisions[decision2Index].decisionButtonText;
            textIsActive = false;
            buttonIsActive = false;
        }

        if (endofEvent)
        {
            eventUI.SetActive(false);
            endofEvent = false;
            enabled = false;
        }
    }

    public void Decision1()
    {
        decision1Button.SetActive(false);
        decision2Button.SetActive(false);
        eventText.text = eventManager.ActiveEvent.Decisions[decision1Index].consequenceText;
        for (int i = 0; i < eventManager.ActiveEvent.Decisions[decision1Index].consequenceOnResources.Length; i++)
        {
            switch (eventManager.ActiveEvent.Decisions[decision1Index].consequenceOnResources[i].resource)
            {
                case ResourceTypes.Material:
                    multiResourceManager = MainManagerSingleton.Instance.GetComponent<MaterialManager>();
                    break;
                case ResourceTypes.Energy:
                    multiResourceManager = MainManagerSingleton.Instance.GetComponent<EnergyManager>();
                    break;
                //case ResourceTypes.Citizen:
                //    multiResourceManager = MainManagerSingleton.Instance.GetComponent<CitizenManager>();
                //    break;
                case ResourceTypes.Food:
                    multiResourceManager = MainManagerSingleton.Instance.GetComponent<FoodManager>();
                    break;
                case ResourceTypes.Water:
                    multiResourceManager = MainManagerSingleton.Instance.GetComponent<WaterManager>();
                    break;
                default:
                    break;
            }
        }
        endofEvent = true;
    }

    public void Decision2()
    {
        decision1Button.SetActive(false);
        decision2Button.SetActive(false);
        eventText.text = eventManager.ActiveEvent.Decisions[decision2Index].consequenceText;
        endofEvent = true;
    }
}
