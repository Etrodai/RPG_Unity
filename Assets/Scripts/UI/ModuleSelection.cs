using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class ModuleSelection : MonoBehaviour
{
    [SerializeField] private GameObject prefabBlueprint;
    private int indexOffsetX;
    private int indexOffsetY;
    private int indexOffsetZ;

    private void Awake()
    {
        indexOffsetX = (int)Gridsystem.Instance.GridSizeXYZ.x / 2;
        indexOffsetY = (int)Gridsystem.Instance.GridSizeXYZ.y / 2;
        indexOffsetZ = (int)Gridsystem.Instance.GridSizeXYZ.z / 2;
    }

    private void OnMouseDown()
    {
        GameObject clickedModule = gameObject;
        int indX = (int) clickedModule.transform.position.x + indexOffsetX; //Not a safe Method when transform is not accurate, change method
        int indY = (int) clickedModule.transform.position.y + indexOffsetY;
        int indZ = (int) clickedModule.transform.position.z + indexOffsetZ;
        
        //Activate Grid around clicked Object
        Gridsystem.Instance.tileArray[indX + 1, indY, indZ].SetActive(true);
        Gridsystem.Instance.tileArray[indX - 1, indY, indZ].SetActive(true);
        
        Gridsystem.Instance.tileArray[indX, indY + 1, indZ].SetActive(true);
        Gridsystem.Instance.tileArray[indX, indY - 1, indZ].SetActive(true);
        
        Gridsystem.Instance.tileArray[indX, indY, indZ + 1].SetActive(true);
        Gridsystem.Instance.tileArray[indX, indY, indZ - 1].SetActive(true);
    }
}
