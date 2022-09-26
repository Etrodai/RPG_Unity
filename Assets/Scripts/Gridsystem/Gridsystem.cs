using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Gridsystem : MonoBehaviour
{
    [SerializeField] private GameObject prefabCube;
    [SerializeField] private GameObject prefabStation;
    [SerializeField] private Vector3 gridSizeXYZ = new Vector3(0,0,0);

    public Vector3 spawnPos = Vector3.zero;

    private void Awake()
    {
        InitGrid();
    }

    private void InitGrid()
    {
        //Parent Object as Container
        GameObject gridContainer = new GameObject { name = "GridBox", transform = { position = new Vector3(0, 0, 0) } };

        //Setting Tiles in Grid
        //Z-Axis
        /*
        for (int z = 0; z < gridSizeXYZ.z; z++)
        {
            //Y-Axis
            for (int y = 0; y < gridSizeXYZ.y; y++)
            {
                //X-Axis
                for (int x = 0; x < gridSizeXYZ.x; x++)
                {
                    //Instantiate 1 GridTile
                    spawnPos = new Vector3(x - (gridSizeXYZ.x/2), y, z -(gridSizeXYZ.z/2));
                    GameObject gridTile = Instantiate(prefabCube, spawnPos, Quaternion.identity);
                    
                    //Putting in Container
                    gridTile.transform.parent = gridContainer.transform;
                    gridTile.name = $"Tile_{x}.{y}.{z}";
                    
                    //GridTile Script for further Actions
                    gridTile.AddComponent<GridTile>();
                }
            }
        }
        */
        
        //Initial Station Spawn
        
        //spawnPos = new Vector3(-0.5f,(gridSizeXYZ.y-1) / 2, -0.5f);   //System with predefined Grid
        
        GameObject centerTile = Instantiate(prefabStation, spawnPos, Quaternion.identity);

        centerTile.name = "Station";
        centerTile.tag = "Station";
    }
}
