using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PirateAttack : BaseEvent
{
    private EventManager eventManager;
    private CitizenManager citizenManager;
    private FoodManager foodManager;
    private WaterManager waterManager;
    private EnergyManager energyManager;
    private MaterialManager materialManager;

    private int dialogCaseIndex;
    private int dialogTextIndex;

    private const int resetIndex = 0;

    private bool canClick;

    [SerializeField]
    private TextMeshProUGUI text;

    [SerializeField]
    private string[] eventText;

    [SerializeField]
    private GameObject[] gameObjects;

    private void Start()
    {
        gameObjects[0].SetActive(false);
        gameObjects[1].SetActive(false);
        gameObjects[2].SetActive(false);
        gameObjects[3].SetActive(false);
        dialogCaseIndex = resetIndex;
        dialogTextIndex = resetIndex;
        eventManager = EventManager.Instance;
        citizenManager = CitizenManager.Instance;
        foodManager = FoodManager.Instance;
        waterManager = WaterManager.Instance;
        energyManager = EnergyManager.Instance;
        materialManager = MaterialManager.Instance;
        eventManager.AvailableEvents.Add(this);
        canClick = true;
        this.enabled = false;
    }

    private void OnEnable()
    {
        dialogCaseIndex = resetIndex;
        dialogTextIndex = resetIndex;
        canClick = true;
        gameObjects[0].SetActive(true);
        EventBehaviour();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && canClick)
        {
            dialogCaseIndex++;
            EventBehaviour();
        }
    }

    protected override void EventBehaviour()
    {
        switch (dialogCaseIndex) //Needs rework since most of the executes can be done in the same case
        {
            case 0:
                text.text = eventText[dialogTextIndex];
                dialogTextIndex++;
                break;
            case 1:
                gameObjects[1].SetActive(true);
                gameObjects[2].SetActive(true);
                text.text = eventText[dialogTextIndex]; //dialogText will be increased via UI button presses
                canClick = false;
                break;
            case 2:
                gameObjects[1].SetActive(false);
                gameObjects[2].SetActive(false);
                gameObjects[3].SetActive(true);
                text.text = eventText[dialogTextIndex]; //Event will be stopped via accept button
                break;
            default:
                break;
        }
    }

    public override void ClickDecision1() //Maybe needs if check in case material is not enough
    {
        //75% chance for 40% of all resources
        //System.Random random = new System.Random();
        //random.Next(1, 4);
        
        foodManager.SavedResourceValue -= foodManager.SaveSpace * 0.4f;
        waterManager.SavedResourceValue -= waterManager.SaveSpace * 0.4f;
        energyManager.SavedResourceValue -= energyManager.SaveSpace * 0.4f;
        materialManager.SavedResourceValue -= materialManager.SaveSpace * 0.4f;

        dialogCaseIndex++;
        dialogTextIndex++;
        EventBehaviour();
    }

    public override void ClickDecision2() //Shouldn't require if check as it is the worse decision
    {
        //25% citizen loss

        citizenManager.Citizen -= (int)(citizenManager.Citizen * 0.25f);
        dialogCaseIndex++;
        dialogTextIndex += 2;
        EventBehaviour();
    }

    public override void AcceptDecision()
    {
        gameObjects[1].SetActive(false);
        gameObjects[2].SetActive(false);
        gameObjects[3].SetActive(false);
        gameObjects[0].SetActive(false);
        eventManager.Action.Invoke(); //Call of the ResetTimer method
        this.enabled = false; //Deactivation of the event
    }
}
