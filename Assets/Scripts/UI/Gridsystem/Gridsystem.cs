using System.Collections.Generic;
using SolarSystem;
using TMPro;
using UnityEngine;

namespace UI.Gridsystem
{
    public class Gridsystem : MonoBehaviour //Made by Ben
    {
        [SerializeField] private Transform solarCenterPoint; //TODO: (Ben) Init with Vector directly

        //Prefabs
        [SerializeField] private GameObject prefabGridTile;
        [SerializeField] private GameObject prefabStation;

        //Singleton

        public static Gridsystem Instance { get; private set; }

        private GameObject gridContainer;

        public GameObject CenterTile { get; private set; }

        //Grid
        [SerializeField, Tooltip("Use value above Zero")]
        private Vector3 gridSizeXYZ = new Vector3(0, 0, 0);

        private Vector3 GridSizeXYZ => gridSizeXYZ; //Set is not required ingame

        private Vector3 spawnPos = Vector3.zero;
        public bool gridIsVisible = true;
        public bool isTextVisible;

        //Workaround in static or Singleton?

        public GridTile[,,] TileArray { get; private set; }

        public List<GridTile> currentVisibleTileList = new();

        /// <summary>
        /// Create Instance and Initialing Grid
        /// </summary>
        private void Awake()
        {
            SingletonGridsystem();
            CheckGridSize();
            InitGrid();
        }

        private void SingletonGridsystem()
        {
            if (Instance != null && Instance != this)
                Destroy(this.gameObject);
            else
                Instance = this;
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
                        if (!TileArray[x, y, z].HasModule)
                        {
                            continue;
                        }

                        //X-Axis
                        if ((x + 1) < TileArray.GetLength(0))
                            SelectNearGridTile(x + 1, y, z);
                        if (x != 0)
                            SelectNearGridTile(x - 1, y, z);
                        //Y-Axis
                        if ((y + 1) < TileArray.GetLength(1))
                            SelectNearGridTile(x, y + 1, z);
                        if (y != 0)
                            SelectNearGridTile(x, y - 1, z);
                        //Z-Axis
                        if ((z + 1) < TileArray.GetLength(2))
                            SelectNearGridTile(x, y, z + 1);
                        if (z != 0)
                            SelectNearGridTile(x, y, z - 1);
                    }
                }
            }
        }

        private void SelectNearGridTile(int xValue, int yValue, int zValue)
        {
            if (TileArray[xValue, yValue, zValue].HasModule)
                return;
            TileArray[xValue, yValue, zValue].ChangeActiveState(true);
            currentVisibleTileList.Add(TileArray[xValue, yValue, zValue]);
        }

        public void UnCheckAvailableGridTilesAroundStation(bool isBuilt)
        {
            if (currentVisibleTileList.Count == 0)
                return;


            for (int i = 0; i < currentVisibleTileList.Count; i++)
            {
                currentVisibleTileList[i].ChangeActiveState(false);
                if (!isBuilt)
                    currentVisibleTileList[i].IsLocked = false;
            }

            currentVisibleTileList.Clear();
        }


        #region Initializing

        private void InitGrid()
        {
            TileArray = new GridTile[(int)GridSizeXYZ.x, (int)GridSizeXYZ.y, (int)GridSizeXYZ.z];

            //Parent Objects as Container
            gridContainer = new GameObject { name = "GridBox", transform = { position = spawnPos } };
            GameObject grid = new GameObject { name = "Grid", transform = { position = spawnPos } };
            grid.transform.parent = gridContainer.transform;

            int arrayIndex = 0; //TODO: is it used
            Vector3 offSetVector = new Vector3(Mathf.Floor(GridSizeXYZ.x / 2), Mathf.Floor(GridSizeXYZ.y / 2),
                Mathf.Floor(GridSizeXYZ.z / 2));

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
                        spawnPos = new Vector3(x - offSetVector.x, y - offSetVector.y, z - offSetVector.z);
                        GameObject gridTile = Instantiate(prefabGridTile, spawnPos, Quaternion.identity);

                        //Putting in Container
                        gridTile.transform.parent = grid.transform;
                        gridTile.name = $"Tile_{spawnPos.x}.{spawnPos.y}.{spawnPos.z}";

                        GridTile gridTileScript = gridTile.GetComponent<GridTile>();
                        TextMeshPro gridText = gridTile.transform.GetChild(0).GetComponent<TextMeshPro>();
                        if (isTextVisible) //TODO: (Ben) As Ternary Operator
                        {
                            gridText.enabled = isTextVisible;
                            gridText.text = $"Tile_{spawnPos.x}.{spawnPos.y}.{spawnPos.z}";
                        }
                        else
                            gridText.enabled = isTextVisible;


                        if (spawnPos.x == 0 && spawnPos.y == 0 && spawnPos.z == 0)
                        {
                            gridTileScript.HasModule = true;
                        } //TODO: (Ben) Better Method to Init startGridTile

                        if (gridIsVisible)
                            gridTileScript.InitActiveState();
                        //Save GridFile in Array
                        TileArray[x, y, z] = gridTileScript;
                    }
                }
            }

            InitStation(gridContainer.transform);
        }

        private void SetOrbiting(GameObject gridContainerGameObject)  //TODO: is it used
        {
            Orbiting orbit = gridContainerGameObject.AddComponent<Orbiting>();
            orbit.centerPoint = solarCenterPoint;
            orbit.xSpread = 750;
            orbit.zSpread = 750;
            orbit.rotSpeed = 0.2f;
        }

        private void InitStation(Transform gridContainerTransform)
        {
            GameObject stationContainer = new GameObject { name = "StationBox", transform = { position = spawnPos } };
            stationContainer.transform.parent = gridContainerTransform.transform;
            CenterTile = Instantiate(prefabStation, Vector3.zero, Quaternion.identity);

            CenterTile.name = "Station";
            CenterTile.tag = "Station";
            CenterTile.transform.parent = stationContainer.transform;
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
            //TODO: (Ben) Access Camera to new GridTiles
            CheckGridSize(); 
            TileArray = new GridTile[(int)GridSizeXYZ.x, (int)GridSizeXYZ.y, (int)GridSizeXYZ.z];
            InitGrid();
        }

        #endregion
    }
}