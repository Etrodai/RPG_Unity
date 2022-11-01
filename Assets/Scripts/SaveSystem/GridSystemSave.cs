using System.Collections;
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
        public Vector3 position;
        public BuildingData buildingData;
    }
    
    public class GridSystemSave : MonoBehaviour
    {
        private static List<GridSystemData> data { get; set; }
        private Gridsystem gridsystem;

        private void Start()
        {
            gridsystem = Gridsystem.Instance;
            Save.OnSaveButtonClick.AddListener(SaveData);
            Save.OnSaveAsButtonClick.AddListener(SaveDataAs);
            Load.OnLoadButtonClick.AddListener(LoadData);
        }

        private void SaveData()
        {
            for (int x = 0; x < gridsystem.TileArray.GetLength(0); x++)
            {
                for (int y = 0; y < gridsystem.TileArray.GetLength(1); y++)
                {
                    for (int z = 0; z < gridsystem.TileArray.GetLength(2); z++)
                    {
                        if (!gridsystem.TileArray[x, y, z].HasModule) continue;
                        
                        GridSystemData gridData = new GridSystemData
                        {
                            position = new Vector3(x, y, z),
                            buildingData = gridsystem.TileArray[x, y, z].Module.GetComponent<Building>().SaveBuildingData()
                        };

                        data.Add(gridData);
                    }
                }
            }

            Save.AutoSaveData(data, "GridSystem");
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
                        
                        GridSystemData gridData = new GridSystemData
                        {
                            position = new Vector3(x, y, z),
                            buildingData = gridsystem.TileArray[x, y, z].Module.GetComponent<Building>().SaveBuildingData()
                        };

                        data.Add(gridData);
                    }
                }
            }
        
            Save.SaveDataAs(savePlace, data, "GridSystem");
        }
    
        private void LoadData(string path)
        {
            path = Path.Combine(path, "GridSystem");

            GridSystemData[] gridData = Load.LoadData(path);
        
            // TODO über ResourceOrdner die Buildings wieder hinzufügen
        }
    }
}