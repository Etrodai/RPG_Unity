using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New MileStone", menuName = "MileStones")]
public class MileStonesScriptableObject : ScriptableObject
{
    // Kamerasteuerung, ChangePrioListe                                         TODO
    
    [SerializeField][TextArea] private string[] mileStoneText;
    [SerializeField] private string requiredEvent;                              //TODO
    [SerializeField] private Resource[] requiredResources;
    [SerializeField] private MileStoneModules[] requiredModules;
    [SerializeField][TextArea] private string[] mileStoneAchievedText;

    public string[] MileStoneText => mileStoneText;

    public string RequiredEvent => requiredEvent;

    public Resource[] RequiredResources => requiredResources;

    public MileStoneModules[] RequiredModules => requiredModules;

    public string[] MileStoneAchievedText => mileStoneAchievedText;
}