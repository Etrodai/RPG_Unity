using System.Collections.Generic;
using Buildings;
using UI.Gridsystem;
using UnityEngine;
// ReSharper disable PossibleLossOfFraction

namespace SaveSystem
{
    [System.Serializable]
    public struct GridSystemData
    {
        public int xIndex;
        public int yIndex;
        public int zIndex;
        public BuildingData buildingData;
    }
    
    public class GridSystemSave : MonoBehaviour //Made by Robin
    {
        #region Variables

        private static List<GridSystemData> Data { get; set; }
        private Gridsystem gridsystem;
        private Transform parent;
        private SaveData saveData;

        #endregion

        #region UnityEvents

        private void Start()
        {
            saveData = SaveSystem.SaveData.Instance;
            gridsystem = Gridsystem.Instance;
            Save.OnSaveButtonClick.AddListener(SaveData);
            Save.OnSaveAsButtonClick.AddListener(SaveDataAs);
            Load.OnLoadButtonClick.AddListener(LoadData);
        }

        private void OnDestroy()
        {
            Save.OnSaveButtonClick.RemoveListener(SaveData);
            Save.OnSaveAsButtonClick.RemoveListener(SaveDataAs);
            Load.OnLoadButtonClick.RemoveListener(LoadData);
        }

        #endregion

        #region Save & Load

        private void SaveData()
        { 
            Data = new ();
            for (int x = 0; x < gridsystem.TileArray.GetLength(0); x++)
            {
                for (int y = 0; y < gridsystem.TileArray.GetLength(1); y++)
                {
                    for (int z = 0; z < gridsystem.TileArray.GetLength(2); z++)
                    {
                        if (!gridsystem.TileArray[x, y, z].HasModule) continue;
                        if (gridsystem.TileArray[x,y,z].Module == null) continue;

                        GridSystemData gridData = new();
                        gridData.xIndex = x;
                        gridData.yIndex = y;
                        gridData.zIndex = z;
                        gridData.buildingData = gridsystem.TileArray[x, y, z].Module.GetComponent<Building>().SaveBuildingData();

                        Data.Add(gridData);
                    }
                }
            }

            if (Data.Count == 0) return;

            saveData.GameSave.gridData = Data.ToArray();
        }
    
        private void SaveDataAs(string savePlace)
        {
            Data = new ();
            for (int x = 0; x < gridsystem.TileArray.GetLength(0); x++)
            {
                for (int y = 0; y < gridsystem.TileArray.GetLength(1); y++)
                {
                    for (int z = 0; z < gridsystem.TileArray.GetLength(2); z++)
                    {
                        if (!gridsystem.TileArray[x, y, z].HasModule) continue;
                        if (gridsystem.TileArray[x,y,z].Module == null) continue;

                        GridSystemData gridData = new();
                        
                        gridData.xIndex = x;
                        gridData.yIndex = y;
                        gridData.zIndex = z;
                        gridData.buildingData = gridsystem.TileArray[x, y, z].Module.GetComponent<Building>().SaveBuildingData();

                        Data.Add(gridData);
                    }
                }
            }
        
            if (Data.Count == 0) return;
            
            saveData.GameSave.gridData = Data.ToArray();
        }
    
        private void LoadData(GameSave gameSave)
        {
            if (gameSave.gridData == null) return;
            
            GridSystemData[] gridData = gameSave.gridData;
            Vector3 offSetVector = new Vector3(Mathf.Floor(gridsystem.TileArray.GetLength(0) / 2), 
                                               Mathf.Floor(gridsystem.TileArray.GetLength(1) / 2),
                                               Mathf.Floor(gridsystem.TileArray.GetLength(2) / 2));

            parent = gridsystem.CenterTile.transform;
            
            foreach (GridSystemData data in gridData)
            {
                GameObject buildingGameObject;
                switch ((BuildingType)data.buildingData.buildingType)
                {
                    case BuildingType.EnergyGain:
                        buildingGameObject = Instantiate(Resources.Load("Buildings/pref_EnergyGainModule",
                            typeof(GameObject)), new Vector3(data.xIndex, data.yIndex,data.zIndex)
                                                 - offSetVector, Quaternion.identity, parent) as GameObject;
                        break;
                    case BuildingType.LifeSupportGain:
                        buildingGameObject = Instantiate(Resources.Load("Buildings/pref_FoodWaterGainModule",
                            typeof(GameObject)), new Vector3(data.xIndex, data.yIndex,data.zIndex)
                                                 - offSetVector, Quaternion.identity, parent) as GameObject;
                        break;
                    case BuildingType.MaterialGain:
                        buildingGameObject = Instantiate(Resources.Load("Buildings/pref_WorkGainModule", 
                            typeof(GameObject)), new Vector3(data.xIndex, data.yIndex,data.zIndex)
                                                 - offSetVector, Quaternion.identity, parent) as GameObject;
                        break;
                    case BuildingType.EnergySave:
                        buildingGameObject = Instantiate(Resources.Load("Buildings/pref_EnergyGainModule", 
                            typeof(GameObject)), new Vector3(data.xIndex, data.yIndex,data.zIndex) 
                                                 - offSetVector, Quaternion.identity, parent) as GameObject;
                        break;
                    case BuildingType.LifeSupportSave:
                        buildingGameObject = Instantiate(Resources.Load("Buildings/pref_FoodWaterSaveModule", 
                            typeof(GameObject)), new Vector3(data.xIndex, data.yIndex,data.zIndex)
                                                 - offSetVector, Quaternion.identity, parent) as GameObject;
                        break;
                    case BuildingType.MaterialSave:
                        buildingGameObject = Instantiate(Resources.Load("Buildings/pref_WorkSaveModule", 
                            typeof(GameObject)), new Vector3(data.xIndex, data.yIndex,data.zIndex)
                                                 - offSetVector, Quaternion.identity, parent) as GameObject;
                        break;
                    case BuildingType.CitizenSave:
                        buildingGameObject = Instantiate(Resources.Load("Buildings/pref_HousingModule", 
                            typeof(GameObject)), new Vector3(data.xIndex, data.yIndex,data.zIndex)
                                                 - offSetVector, Quaternion.identity, parent) as GameObject;
                        break;
                    default:
                        buildingGameObject = null;
                        break;
                }

                if (buildingGameObject != null)
                {
                    Building buildingScript = buildingGameObject.GetComponent<Building>();
                    buildingScript.IsLoading = true;
                    buildingScript.CurrentProductivity = data.buildingData.currentProductivity;
                    buildingScript.IsDisabled = data.buildingData.isDisabled;
                }

                gridsystem.TileArray[data.xIndex, data.yIndex, data.zIndex].HasModule = true;
                gridsystem.TileArray[data.xIndex, data.yIndex, data.zIndex].Module = buildingGameObject;
            }
        }

        #endregion
    }
}