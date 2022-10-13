using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Gridsystem : MonoBehaviour
{
    //Singleton
    private static Gridsystem instance;

    public static Gridsystem Instance
    {
        get => instance;
    }

    //Prefabs
    [SerializeField] private GameObject prefabGridTile;
    [SerializeField] private GameObject prefabStation;

    //Grid
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

    public List<GridTile> currentVisibleTileList = new List<GridTile>();


    /// <summary>
    /// Create Instance and Initialing Grid
    /// </summary>
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

        //Initializing
        CheckGridSize();
        tileArray = new GridTile[(int)GridSizeXYZ.x, (int)GridSizeXYZ.y, (int)GridSizeXYZ.z];
        InitGrid();
    }


    /// <summary>
    /// 
    /// </summary>
    public void CheckAvailableGridTilesAroundStation()
    {
        for (int z = 0; z < GridSizeXYZ.z; z++)
        {
            for (int y = 0; y < GridSizeXYZ.y; y++)
            {
                for (int x = 0; x < GridSizeXYZ.x; x++)
                {
                    //Check only when Tile is locked
                    if (!tileArray[x, y, z].IsLocked)
                    {
                        continue;
                    }


                    //X-Axis

                    if ((x + 1) < tileArray.GetLength(0))
                    {
                        tileArray[x + 1, y, z].ChangeActiveState(true);
                        currentVisibleTileList.Add(tileArray[x + 1, y, z]);
                    }

                    if (x != 0)
                    {
                        tileArray[x - 1, y, z].ChangeActiveState(true);
                        currentVisibleTileList.Add(tileArray[x - 1, y, z]);
                    }

                    //Y-Axis
                    if ( (y + 1) < tileArray.GetLength(1))
                    {
                        tileArray[x, y + 1, z].ChangeActiveState(true);
                        currentVisibleTileList.Add(tileArray[x, y + 1, z]);
                    }

                    if (y != 0)
                    {
                        tileArray[x, y - 1, z].ChangeActiveState(true);
                        currentVisibleTileList.Add(tileArray[x, y - 1, z]);
                    }

                    //Z-Axis
                    if ( (z + 1) < tileArray.GetLength(2))
                    {
                        tileArray[x, y, z + 1].ChangeActiveState(true);
                        currentVisibleTileList.Add(tileArray[x, y, z + 1]);
                    }

                    if (z != 0)
                    {
                        tileArray[x, y, z - 1].ChangeActiveState(true);
                        currentVisibleTileList.Add(tileArray[x, y, z - 1]);
                    }
                }
            }
        }
    }

    public void UnCheckAvailableGridTilesAroundStation()
    {
        if (currentVisibleTileList.Count == 0)
        {
            return;
        }

        for (int i = 0; i < currentVisibleTileList.Count; i++)
        {
            currentVisibleTileList[i].ChangeActiveState(false);
            currentVisibleTileList[i].IsLocked = false;
        }

        currentVisibleTileList.Clear();
    }

    public void UnCheckAvailableGridTilesAroundStation(bool isBuilded)
    {
        if (currentVisibleTileList.Count == 0)
        {
            return;
        }

        for (int i = 0; i < currentVisibleTileList.Count; i++)
        {
            currentVisibleTileList[i].ChangeActiveState(false);
            if (!isBuilded)
            {
                currentVisibleTileList[i].IsLocked = false;
            }
        }

        currentVisibleTileList.Clear();
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
                    spawnPos = new Vector3(x - Mathf.Floor(GridSizeXYZ.x / 2), y - Mathf.Floor(GridSizeXYZ.y / 2),
                        z - Mathf.Floor(GridSizeXYZ.z / 2));
                    GameObject gridTile = Instantiate(prefabGridTile, spawnPos, Quaternion.identity);

                    //Putting in Container
                    gridTile.transform.parent = gridContainer.transform;
                    gridTile.name = $"Tile_{spawnPos.x}.{spawnPos.y}.{spawnPos.z}";

                    GridTile gridTileScript = gridTile.GetComponent<GridTile>();

                    if (gridIsVisible)
                    {
                        gridTileScript.InitActiveState();
                    }

                    //Save GridFile in Array
                    tileArray[x, y, z] = gridTileScript;
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
        tileArray = new GridTile[(int)GridSizeXYZ.x, (int)GridSizeXYZ.y, (int)GridSizeXYZ.z];
        InitGrid();
    }

    #endregion
}