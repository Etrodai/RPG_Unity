using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MeteoricRain : BaseEvent
{
    private EventManager eventManager;
    private MaterialManager materialManager;
    private FoodManager foodManager;
    private WaterManager waterManager;

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
        materialManager = MaterialManager.Instance;
        foodManager = FoodManager.Instance;
        waterManager = WaterManager.Instance;
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
        if(Input.GetMouseButtonDown(0) && canClick)
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
                text.text = eventText[dialogTextIndex];
                dialogTextIndex++;
                break;
            case 2:
                gameObjects[1].SetActive(true);
                gameObjects[2].SetActive(true);
                text.text = eventText[dialogTextIndex]; //dialogText will be increased via UI button presses
                canClick = false;
                break;
            case 3:
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
        //25% material loss
        materialManager.SavedResourceValue -= materialManager.SaveSpace * 0.25f;
        dialogCaseIndex++;
        dialogTextIndex++;
        EventBehaviour();
    }

    public override void ClickDecision2() //Shouldn't require if check as it is the worse decision
    {
        //50% chance for 30% food and 10% material loss
        //System.Random random = new System.Random();
        //random.Next(1, 2);

        foodManager.SavedResourceValue -= foodManager.SaveSpace * 0.30f;
        waterManager.SavedResourceValue -= waterManager.SaveSpace * 0.30f;
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
