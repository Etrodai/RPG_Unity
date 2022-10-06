using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event1Test : BaseEvent //Probably have to be deactivated at the beginning and just be activated when chosen as next event
{
    private EventManager eventManager;

    private int dialogTextIndex;
    private const int resetIndex = 0;        //all three fields can probably be changed to override properties

    private void Start()
    {
        dialogTextIndex = resetIndex;
        eventManager = EventManager.Instance;
        eventManager.AvailableEvents.Add(this);
        this.enabled = false;
    }

    private void OnEnable()
    {
        dialogTextIndex = resetIndex;
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            dialogTextIndex++;
            EventBehaviour();
        }
    }

    protected override void EventBehaviour()
    {
        switch (dialogTextIndex)
        {
            case 0:
                Debug.Log("Case 0 executed");
                //Erster Dialog + Text UI anzeigen
                break;
            case 1:
                eventManager.Action.Invoke(); //Call of the ResetTimer method
                this.enabled = false; //Deactivation of the event
                break;
            default:
                break;
        }
    }
}
