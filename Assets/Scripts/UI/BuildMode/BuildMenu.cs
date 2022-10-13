using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class BuildMenu : MonoBehaviour
{
    [SerializeField] private GameObject buildmenuLayout;
    private Camera mainCam;

    [SerializeField] private GameObject prefab_blueprintObject;
    private GameObject blueprintObject;
    private GameObject moduleToBuild;
    
    Vector3 mousePos;

    private void Start()
    {
        mainCam = Camera.main;
    }

    /// <summary>
    /// De/Activate Build Menu
    /// </summary>
    public void RightMouseButtonPressed()
    {
        buildmenuLayout.SetActive(!buildmenuLayout.activeSelf);

        if (buildmenuLayout.activeSelf)
        {
        mousePos = Input.mousePosition;
        buildmenuLayout.transform.position = mousePos;
        }
        else
        {
            Destroy(blueprintObject);
            UnCheckAvailableGridTiles(false);
        }
    }

    public void LeftMouseButtonPressed()
    {
        if (!buildmenuLayout.activeSelf || blueprintObject == null)
        {
            return;
        }
        
        Blueprint blueprint = blueprintObject.GetComponent<Blueprint>();
        if (blueprint.IsLockedIn)
        {
            UnCheckAvailableGridTiles(true);
            buildmenuLayout.SetActive(false);

            GameObject module = Instantiate(moduleToBuild, blueprint.transform.position, quaternion.identity);
            module.transform.parent = GameObject.FindGameObjectWithTag("Station").transform; //Redo

            blueprint.gridTileHit.GetComponent<Collider>().isTrigger = false;
            Destroy(blueprintObject);
        }
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="objectToBuild"></param>
    public void BuildMenuButtonPressed(GameObject _moduleToBuild)
    {
        mousePos = Input.mousePosition;
        Vector3 spawnPos = mainCam.ScreenToWorldPoint(mousePos);
        CheckAvailableGridTiles();

        moduleToBuild = _moduleToBuild;
        blueprintObject = Instantiate(prefab_blueprintObject, spawnPos,Quaternion.identity);
    }

    private void CheckAvailableGridTiles()
    {
        Gridsystem.Instance.CheckAvailableGridTilesAroundStation();
    }
    private void UnCheckAvailableGridTiles(bool isBuilded)
    {
        Gridsystem.Instance.UnCheckAvailableGridTilesAroundStation(isBuilded);
    }
}
