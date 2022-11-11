using System.Collections.Generic;
using System.IO;
using Buildings;
using UI.Gridsystem;
using UnityEngine;

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
    
    public class GridSystemSave : MonoBehaviour
    {
        #region Variables

        private static List<GridSystemData> data { get; } = new();
        private Gridsystem gridsystem;
        private const string SaveName = "GridSystem";
        [SerializeField] private Transform parent;

        #endregion

        #region UnityEvents

        private void Start()
        {
            gridsystem = Gridsystem.Instance;
            Save.OnSaveButtonClick.AddListener(SaveData);
            Save.OnSaveAsButtonClick.AddListener(SaveDataAs);
            Load.OnLoadButtonClick.AddListener(LoadData);
        }

        #endregion

        #region Save & Load

         private void SaveData()
        {
            for (int x = 0; x < gridsystem.TileArray.GetLength(0); x++)
            {
                for (int y = 0; y < gridsystem.TileArray.GetLength(1); y++)
                {
                    for (int z = 0; z < gridsystem.TileArray.GetLength(2); z++)
                    {
                        if (!gridsystem.TileArray[x, y, z].HasModule) continue;
                        if (gridsystem.TileArray[x,y,z].Module == null) continue;
                        
                        GridSystemData gridData = new GridSystemData
                        {
                            xIndex = x,
                            yIndex = y,
                            zIndex = y,
                            buildingData = gridsystem.TileArray[x, y, z].Module.GetComponent<Building>().SaveBuildingData()
                        };

                        data.Add(gridData);
                    }
                }
            }

            if (data.Count == 0) return;
            
            Save.AutoSaveData(data, SaveName);
        }
    
        private void SaveDataAs(string savePlace)
        {
            for (int x = 0; x < gridsystem.TileArray.GetLength(0); x++)
            {
                for (int y = 0; y < gridsystem.TileArray.GetLength(1); y++)
                {
                    for (int z = 0; z < gridsystem.TileArray.GetLength(2); z++)
                    {
                        if (!gridsystem.TileArray[x, y, z].HasModule) continue;
                        if (gridsystem.TileArray[x,y,z].Module == null) continue;

                        GridSystemData gridData = new GridSystemData
                        {
                            xIndex = x,
                            yIndex = y,
                            zIndex = y,
                            buildingData = gridsystem.TileArray[x, y, z].Module.GetComponent<Building>().SaveBuildingData()
                        };

                        data.Add(gridData);
                    }
                }
            }
        
            if (data.Count == 0) return;
            
            Save.SaveDataAs(savePlace, data, SaveName);
        }
    
        private void LoadData(string path)
        {
            path = Path.Combine(path, SaveName);
            if (!File.Exists(path)) return;
            
            GridSystemData[] gridData = Load.LoadData(path) as GridSystemData[];
            Vector3 offSetVector = new Vector3(Mathf.Floor(gridsystem.TileArray.GetLength(0) / 2), 
                                               Mathf.Floor(gridsystem.TileArray.GetLength(1) / 2),
                                               Mathf.Floor(gridsystem.TileArray.GetLength(2) / 2));

            foreach (GridSystemData data in gridData)
            {
                GameObject buildingGameObject;
                switch ((BuildingTypes)data.buildingData.buildingType)
                {
                    case BuildingTypes.EnergyGain:
                        buildingGameObject = Instantiate(Resources.Load("Buildings/pref_EnergyGainModule",
                            typeof(GameObject)), new Vector3(data.xIndex, data.yIndex,data.zIndex)
                                                 - offSetVector, Quaternion.identity, parent) as GameObject;
                        break;
                    case BuildingTypes.LifeSupportGain:
                        buildingGameObject = Instantiate(Resources.Load("Buildings/pref_FoodWaterGainModule",
                            typeof(GameObject)), new Vector3(data.xIndex, data.yIndex,data.zIndex)
                                                 - offSetVector, Quaternion.identity, parent) as GameObject;
                        break;
                    case BuildingTypes.MaterialGain:
                        buildingGameObject = Instantiate(Resources.Load("Buildings/pref_WorkGainModule", 
                            typeof(GameObject)), new Vector3(data.xIndex, data.yIndex,data.zIndex)
                                                 - offSetVector, Quaternion.identity, parent) as GameObject;
                        break;
                    case BuildingTypes.EnergySave:
                        buildingGameObject = Instantiate(Resources.Load("Buildings/pref_EnergyGainModule", 
                            typeof(GameObject)), new Vector3(data.xIndex, data.yIndex,data.zIndex) 
                                                 - offSetVector, Quaternion.identity, parent) as GameObject;
                        break;
                    case BuildingTypes.LifeSupportSave:
                        buildingGameObject = Instantiate(Resources.Load("Buildings/pref_FoodWaterSaveModule", 
                            typeof(GameObject)), new Vector3(data.xIndex, data.yIndex,data.zIndex)
                                                 - offSetVector, Quaternion.identity, parent) as GameObject;
                        break;
                    case BuildingTypes.MaterialSave:
                        buildingGameObject = Instantiate(Resources.Load("Buildings/pref_WorkSaveModule", 
                            typeof(GameObject)), new Vector3(data.xIndex, data.yIndex,data.zIndex)
                                                 - offSetVector, Quaternion.identity, parent) as GameObject;
                        break;
                    case BuildingTypes.CitizenSave:
                        buildingGameObject = Instantiate(Resources.Load("Buildings/pref_HousingModule", 
                            typeof(GameObject)), new Vector3(data.xIndex, data.yIndex,data.zIndex)
                                                 - offSetVector, Quaternion.identity, parent) as GameObject;
                        break;
                    default:
                        buildingGameObject = null;
                        break;
                }

                Building buildingScript = buildingGameObject.GetComponent<Building>();
                buildingScript.CurrentProductivity = data.buildingData.currentProductivity;
                buildingScript.IsDisabled = data.buildingData.isDisabled;
                gridsystem.TileArray[data.xIndex, data.yIndex, data.zIndex].HasModule = true;
                gridsystem.TileArray[data.xIndex, data.yIndex, data.zIndex].Module = buildingGameObject;
            }
            // TODO: (Robin) wird es an der richtigen Position instantiiert?
        }

        #endregion
    }
}