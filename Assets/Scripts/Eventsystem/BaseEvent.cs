using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class BaseEvent : MonoBehaviour
{
    //public abstract EventManager EventManager { get; set; }

    //public abstract int DialogTextIndex { get; set; }
    //public abstract TextMeshProUGUI Text { get; set; }

    protected abstract void EventBehaviour();

    public abstract void ClickDecision1();
    public abstract void ClickDecision2();
    public abstract void AcceptDecision();
}
