using ResourceManagement.Manager;
using System.Collections;
using System.Collections.Generic;
using ResourceManagement.Manager;
using TMPro;
using UnityEngine;

public class CrossWay : BaseEvent
{
    private EventManager eventManager;
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
                text.text = eventText[dialogTextIndex];
                dialogTextIndex++;
                break;
            case 2:
                text.text = eventText[dialogTextIndex];
                dialogTextIndex++;
                break;
            case 3:
                gameObjects[1].SetActive(true);
                gameObjects[2].SetActive(true);
                text.text = eventText[dialogTextIndex]; //dialogText will be increased via UI button presses
                canClick = false;
                break;
            case 4:
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
        materialManager.SavedResourceValue += materialManager.SaveSpace * 0.25f; //25% material gain
        dialogCaseIndex++;
        dialogTextIndex++;
        EventBehaviour();
    }

    public override void ClickDecision2() //Shouldn't require if check as it is the worse decision
    {
        //Nothing happens
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
