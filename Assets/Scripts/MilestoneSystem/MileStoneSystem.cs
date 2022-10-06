using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


// TODO
// NullChecks for all SO types
// Comments in all scripts
// events for MileStoneSystem (siehe SO)


public class MileStoneSystem : MonoBehaviour
{
    [SerializeField] private MileStonesScriptableObject[] mileStones;
    private int mileStonesDone = 0;
    [SerializeField] private GameObject mainText;
    private int textIndex = 0;
    private bool isDone;
    private TextMeshProUGUI mileStoneText;
    [SerializeField] private GameObject menu;
    private TextMeshProUGUI requiredStuffText;
    bool isMinimized;

    [SerializeField] private bool isAchieved;

    private void Awake()
    {
        mileStoneText = mainText.GetComponentInChildren<TextMeshProUGUI>();
        requiredStuffText = menu.GetComponentInChildren<TextMeshProUGUI>();
        BuildMenu();
        OnClickMenuButton();
        BuildMainText();
    }

    private void BuildMainText()
    {
        mainText.SetActive(true);
        Time.timeScale = 0;
        mileStoneText.text = mileStones[mileStonesDone].MileStoneText[textIndex];
        textIndex++;
    }
    
    private void BuildMenu()
    {
        requiredStuffText.text = mileStones[mileStonesDone].RequiredEvent;
        foreach (Resource item in mileStones[mileStonesDone].RequiredResources)
        {
            requiredStuffText.text += "\n";
            requiredStuffText.text += item.resource;
            requiredStuffText.text += ":    ";
            requiredStuffText.text += item.value;
        }

        foreach (MileStoneModules item in mileStones[mileStonesDone].RequiredModules)
        {
            requiredStuffText.text += "\n";
            requiredStuffText.text += item.buildingTypes;
            requiredStuffText.text += ":    ";
            requiredStuffText.text += item.value;
        }
    }

    private void Update()
    {
        if (isAchieved)                     // CheckIfAchieved()
        {
            isDone = true;
            mainText.SetActive(true);
            Time.timeScale = 0;
            mileStoneText.text = mileStones[mileStonesDone].MileStoneAchievedText[textIndex];
            textIndex++;
            isAchieved = false;
        }
    }

    private bool CheckIfAchieved()                                                  //TODO
    {
        return false;
    }

    public void OnClickMenuButton()
    {
        isMinimized = !isMinimized;
        if (isMinimized)
        {
            var transformPosition = menu.transform.position;
            transformPosition.x += 300;
            menu.transform.position = transformPosition;
        }
        else
        {
            var transformPosition = menu.transform.position;
            transformPosition.x -= 300;
            menu.transform.position = transformPosition;
        }
    }

    public void OnClickOKButton()
    {
        if (isDone)
        {
            if (textIndex < mileStones[mileStonesDone].MileStoneAchievedText.Length)
            {
                mileStoneText.text = mileStones[mileStonesDone].MileStoneAchievedText[textIndex];
                textIndex++;
            }
            else
            {
                textIndex = 0;
                isDone = false;
                mileStonesDone++;
                BuildMainText();
                BuildMenu();
            }
        }
        else
        {
            if (textIndex < mileStones[mileStonesDone].MileStoneText.Length)
            {
                mileStoneText.text = mileStones[mileStonesDone].MileStoneText[textIndex];
                textIndex++;
            }
            else
            {
                mainText.SetActive(false);
                Time.timeScale = 1;
                textIndex = 0;
            }
        }
    }
}
