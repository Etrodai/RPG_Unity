using TMPro;
using UnityEngine;


// TODO
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
    }

    private void Start()
    {
        BuildPreMainText();
    }

    #region MainText
    // immer vor einem Meilenstein
    private void BuildPreMainText()
    {
        if (textIndex < mileStones[mileStonesDone].MileStoneText.Length)
        {
            if (!isMinimized) CloseMenu();
            mainText.SetActive(true);
            Time.timeScale = 0;
            mileStoneText.text = mileStones[mileStonesDone].MileStoneText[textIndex];
            textIndex++;
        }
        else
        {
            mainText.SetActive(false);
            BuildMenu();
            if (isMinimized) OpenMenu();
            Time.timeScale = 1;
            textIndex = 0;
        }
    }
    
    // immer nach einem Meilenstein
    private void BuildPostMainText()
    {
        if (textIndex < mileStones[mileStonesDone].MileStoneAchievedText.Length)
        {
            if (!isMinimized) CloseMenu();
            mainText.SetActive(true);
            Time.timeScale = 0;
            mileStoneText.text = mileStones[mileStonesDone].MileStoneAchievedText[textIndex];
            textIndex++;
        }
        else
        {
            textIndex = 0;
            isDone = false;
            mileStonesDone++;
            BuildPreMainText();
        }
    }
    
    public void OnClickOKButton()
    {
        if (isDone)
        {
            BuildPostMainText();
        }
        else
        {
            BuildPreMainText();
        }
    }
    #endregion
    
    #region Menu
    // immer, wenn ein Meilenstein erreicht wurde
    private void BuildMenu() 
    {
        requiredStuffText.text = mileStones[mileStonesDone].RequiredEvent;
        
        if (mileStones[mileStonesDone].RequiredResources.Length != 0)
        {
            requiredStuffText.text += "\nRequired resources:";
            foreach (Resource item in mileStones[mileStonesDone].RequiredResources)
            {
                requiredStuffText.text += $"\n{item.value} {item.resource}\n";
            }
        }

        if (mileStones[mileStonesDone].RequiredModules.Length != 0)
        {
            requiredStuffText.text += "\nRequired modules:";
            foreach (MileStoneModules item in mileStones[mileStonesDone].RequiredModules)
            {
                requiredStuffText.text += $"\n{item.value} {item.buildingTypes}\n";
            }
        }
    }

    public void OnClickMenuButton()
    {
        if (isMinimized)
        {
            OpenMenu();
        }
        else
        {
            CloseMenu();
        }
    }

    private void OpenMenu()
    {
        var transformPosition = menu.transform.position;
        transformPosition.x -= 300;
        menu.transform.position = transformPosition;
        isMinimized = false;
    }

    private void CloseMenu()
    {
        var transformPosition = menu.transform.position;
        transformPosition.x += 300;
        menu.transform.position = transformPosition;
        isMinimized = true;
    }
    #endregion

    #region CheckIfAchieved

    private void Update()
    {
        // isAchieved = ;
        
        if (CheckIfAchieved())
        {
            // isAchieved = false;
            isDone = true;
            BuildPostMainText();
        }
    }

    private bool CheckIfAchieved()                                                  //TODO
    {
        bool hasAllRequiredStuff = true;
        
        //mileStones[mileStonesDone].RequiredEvent
        
        foreach (Resource item in mileStones[mileStonesDone].RequiredResources)
        {
            switch (item.resource)
            {
                case ResourceTypes.Material:
                    if (MaterialManager.Instance.SavedResourceValue < item.value) hasAllRequiredStuff = false;
                    break;
                case ResourceTypes.Energy:
                    if (EnergyManager.Instance.SavedResourceValue < item.value) hasAllRequiredStuff = false;
                    break;                
                case ResourceTypes.Citizen:
                    if (CitizenManager.Instance.Citizen < item.value) hasAllRequiredStuff = false;
                    break;
                case ResourceTypes.Food:
                    if (FoodManager.Instance.SavedResourceValue < item.value) hasAllRequiredStuff = false;
                    break;
                case ResourceTypes.Water:
                    if (WaterManager.Instance.SavedResourceValue < item.value) hasAllRequiredStuff = false;
                    break;
            }
        }

        foreach (MileStoneModules item in mileStones[mileStonesDone].RequiredModules)
        {
            switch (item.buildingTypes)
            {
                case BuildingTypes.All:
                    if (GameManager.Instance.GetAllBuildingsCount() < item.value) hasAllRequiredStuff = false;
                    break;
                case BuildingTypes.CitizenSave:
                    if (GameManager.Instance.GetBuildingCount(item.buildingTypes) < item.value) hasAllRequiredStuff = false;
                    break;
                case BuildingTypes.EnergyGain:
                    if (GameManager.Instance.GetBuildingCount(item.buildingTypes) < item.value) hasAllRequiredStuff = false;
                    break;
                case BuildingTypes.EnergySave:
                    if (GameManager.Instance.GetBuildingCount(item.buildingTypes) < item.value) hasAllRequiredStuff = false;
                    break;
                case BuildingTypes.MaterialGain:
                    if (GameManager.Instance.GetBuildingCount(item.buildingTypes) < item.value) hasAllRequiredStuff = false;
                    break;
                case BuildingTypes.MaterialSave:
                    if (GameManager.Instance.GetBuildingCount(item.buildingTypes) < item.value) hasAllRequiredStuff = false;
                    break;
                case BuildingTypes.LifeSupportGain:
                    if (GameManager.Instance.GetBuildingCount(item.buildingTypes) < item.value) hasAllRequiredStuff = false;
                    break;
                case BuildingTypes.LifeSupportSave:
                    if (GameManager.Instance.GetBuildingCount(item.buildingTypes) < item.value) hasAllRequiredStuff = false;
                    break;
            }
        }

        return hasAllRequiredStuff;
    }
    #endregion
}
