using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BuildMenu : MonoBehaviour
{
    [SerializeField] private GameObject buildmenuLayout;
    private Camera mainCam;

    [SerializeField] private GameObject modulePrefab;
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
            UnCheckAvailableGridTiles();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="objectToBuild"></param>
    public void BuildMenuButtonPressed(GameObject objectToBuild)
    {
        mousePos = Input.mousePosition;
        Vector3 spawnPos = mainCam.ScreenToWorldPoint(mousePos);
        GameObject blueprintObject = Instantiate(objectToBuild, spawnPos,Quaternion.identity);

        CheckAvailableGridTiles();
        
    }

    private void CheckAvailableGridTiles()
    {
        Gridsystem.Instance.CheckAvailableGridTilesAroundStation();
    }
    private void UnCheckAvailableGridTiles()
    {
        Gridsystem.Instance.UnCheckAvailableGridTilesAroundStation();
    }
    
    //Show available Grid
    //Glue blueprint on Mouse
    //glue in available Grid when on Mouse over
}
