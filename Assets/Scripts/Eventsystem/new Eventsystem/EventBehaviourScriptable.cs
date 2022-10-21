using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EventBehaviourScriptable : MonoBehaviour
{
    //EventBehaviour related variables
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

    public void EventBehaviour() //Method to be called in Player Input Component, must be new script in order to only use it when an event was chosen and activated
    {
        //Texts for duration of length of each text array
        if (textIsActive && textIndex < activeEvent.EventText.Length)
        {
            eventText.text = activeEvent.EventText[textIndex];
            textIndex++;
            buttonIsActive = true;
        }

        if (buttonIsActive && textIndex >= activeEvent.EventText.Length)
        {
            decision1Button.SetActive(true);
            decision2Button.SetActive(true);
            decision1ButtonText.text = activeEvent.Decisions[decision1Index].decisionButtonText;
            decision2ButtonText.text = activeEvent.Decisions[decision2Index].decisionButtonText;
            textIsActive = false;
            buttonIsActive = false;
        }

        if (endofEvent)
        {
            ResetTimer();
            StartCoroutine(NextEventTimer());
            eventUI.SetActive(false);
        }
    }

    public void Decision1()
    {
        decision1Button.SetActive(false);
        decision2Button.SetActive(false);
        eventText.text = activeEvent.Decisions[decision1Index].consequenceText;
        endofEvent = true;
    }

    public void Decision2()
    {
        decision1Button.SetActive(false);
        decision2Button.SetActive(false);
        eventText.text = activeEvent.Decisions[decision2Index].consequenceText;
        endofEvent = true;
    }
}
