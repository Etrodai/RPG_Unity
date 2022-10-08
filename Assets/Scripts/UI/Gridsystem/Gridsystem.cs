using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Gridsystem : MonoBehaviour
{
    private static Gridsystem instance;
    public static Gridsystem Instance { get => instance; }

    [SerializeField] private GameObject prefabGridTile;
    [SerializeField] private GameObject prefabStation;

    [SerializeField, Tooltip("Use value above Zero")] 
    private Vector3 gridSizeXYZ = new Vector3(0, 0, 0);
    public Vector3 GridSizeXYZ
    {
        get => gridSizeXYZ;
        set => gridSizeXYZ = value;
    } //Set is not required ingame

    private Vector3 spawnPos = Vector3.zero;
    public bool gridIsVisible = true;
    
    //Workaround in static or Singleton?
    public GridTile[,,] tileArray;


    
    private void Awake()
    {
        //Singleton
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
        
        
        CheckGridSize();
        //int arraySize = (int)(gridSizeXYZ.x * gridSizeXYZ.y * gridSizeXYZ.z); //When implementing, add arraySize++ in x-for loop below
        tileArray = new GridTile[(int) GridSizeXYZ.x, (int) GridSizeXYZ.y, (int) GridSizeXYZ.z];
        InitGrid();
    }



    public void CheckAvailableGridTilesAroundStation()
    {
        for (int z = 0; z < GridSizeXYZ.z; z++)
        {
            for (int y = 0; y < GridSizeXYZ.y; y++)
            {
                for (int x = 0; x < GridSizeXYZ.x; x++)
                {
                    //Check only when Tile is locked
                    if (!tileArray[x,y,z].IsLocked)
                    {
                        continue;
                    }
                    //X-Axis
                    tileArray[x+1,y,z].SetActive(true);
                    tileArray[x-1,y,z].SetActive(true);
                    //Y-Axis
                    tileArray[x,y+1,z].SetActive(true);
                    tileArray[x,y-1,z].SetActive(true);
                    //Z-Axis
                    tileArray[x,y,z+1].SetActive(true);
                    tileArray[x,y,z-1].SetActive(true);
                    
                }
            }
        }
    }
    
    public void UnCheckAvailableGridTilesAroundStation()
    {
        for (int z = 0; z < GridSizeXYZ.z; z++)
        {
            for (int y = 0; y < GridSizeXYZ.y; y++)
            {
                for (int x = 0; x < GridSizeXYZ.x; x++)
                {
                    //Check only when Tile is locked
                    if (!tileArray[x,y,z].IsLocked)
                    {
                        continue;
                    }
                    //X-Axis
                    tileArray[x+1,y,z].SetActive(false);
                    tileArray[x-1,y,z].SetActive(false);
                    //Y-Axis
                    tileArray[x,y+1,z].SetActive(false);
                    tileArray[x,y-1,z].SetActive(false);
                    //Z-Axis
                    tileArray[x,y,z+1].SetActive(false);
                    tileArray[x,y,z-1].SetActive(false);
                    
                }
            }
        }
    }


    #region Initializing
    private void InitGrid()
    {
        //Parent Object as Container
        GameObject gridContainer = new GameObject { name = "GridBox", transform = { position = spawnPos } };

        int arrayIndex = 0;
        
        //Setting Tiles in Grid
        //Z-Axis
        for (int z = 0; z < GridSizeXYZ.z; z++)
        {
            //Y-Axis
            for (int y = 0; y < GridSizeXYZ.y; y++)
            {
                //X-Axis
                for (int x = 0; x < GridSizeXYZ.x; x++)
                {
                    //Instantiate 1 GridTile
                        //spawnPos = new Vector3(x - (gridSizeXYZ.x / 2), y, z - (gridSizeXYZ.z / 2));
                        spawnPos = new Vector3(x - Mathf.Floor(GridSizeXYZ.x/2), y - Mathf.Floor(GridSizeXYZ.y/2), z - Mathf.Floor(GridSizeXYZ.z/2));
                    GameObject gridTile = Instantiate(prefabGridTile, spawnPos, Quaternion.identity);
                    
                    //Putting in Container
                    gridTile.transform.parent = gridContainer.transform;
                    gridTile.name = $"Tile_{spawnPos.x}.{spawnPos.y}.{spawnPos.z}";

                    GridTile gridTileScript = gridTile.GetComponent<GridTile>();
                    gridTileScript.SetActive(gridIsVisible);
                    gridTileScript.SetActive(gridIsVisible);
                    
                    //Save GridFile in Array
                    tileArray[x,y,z] = gridTileScript;
                }
            }
        }
        
        //Initial Station Spawn
        //spawnPos = new Vector3(-0.5f, (gridSizeXYZ.y - 1) / 2, -0.5f); //System with predefined Grid

        GameObject centerTile = Instantiate(prefabStation, Vector3.zero, Quaternion.identity);

        centerTile.name = "Station";
        centerTile.tag = "Station";
        centerTile.transform.parent = gridContainer.transform;
    }

    /// <summary>
    /// Sets the grid to a uneven Number
    /// </summary>
    private void CheckGridSize()
    {
        // X
        if (GridSizeXYZ.x % 2 == 0)
        {
            gridSizeXYZ.x += 1;
        }

        // Y
        if (GridSizeXYZ.y % 2 == 0)
        {
            gridSizeXYZ.y += 1;
        }

        // Z
        if (GridSizeXYZ.z % 2 == 0)
        {
            gridSizeXYZ.z += 1;
        }
    }

    /// <summary>
    /// Reinit for Editor
    /// </summary>
    public void ReInitialize()
    {
        spawnPos = Vector3.zero;
        CheckGridSize();
        tileArray = new GridTile[(int) GridSizeXYZ.x, (int) GridSizeXYZ.y, (int) GridSizeXYZ.z];
        InitGrid();
    }
    
    #endregion
}