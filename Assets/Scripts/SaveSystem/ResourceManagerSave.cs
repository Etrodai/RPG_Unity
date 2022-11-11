using System.IO;
using Manager;
using ResourceManagement;
using UnityEngine;

namespace SaveSystem
{
    public class ResourceManagerSave : MonoBehaviour
    {
        private float[] managerData;
        private ResourceManager[] managers;
        private const string saveName = "ResourceManager";

        private void Start()
        {
            managers = new ResourceManager[5];
            managers[0] = MainManagerSingleton.Instance.CitizenManager;
            managers[1] = MainManagerSingleton.Instance.EnergyManager;
            managers[2] = MainManagerSingleton.Instance.FoodManager;
            managers[3] = MainManagerSingleton.Instance.MaterialManager;
            managers[4] = MainManagerSingleton.Instance.WaterManager;
            Save.OnSaveButtonClick.AddListener(SaveData);
            Save.OnSaveAsButtonClick.AddListener(SaveDataAs);
            Load.OnLoadButtonClick.AddListener(LoadData);
        }
        
        private void SaveData()
        {
            managerData = new float[5];
            for (int i = 0; i < managers.Length; i++)
            {
                managerData[i] = managers[i].SavedResourceValue;
            }
            Save.AutoSaveData(managerData, saveName);
        }
        
        private void SaveDataAs(string savePlace)
        {
            managerData = new float[5];
            for (int i = 0; i < managers.Length; i++)
            {
                managerData[i] = managers[i].SavedResourceValue;
            }
            Save.SaveDataAs(savePlace, managerData, saveName);
        }

        private void LoadData(string path)
        {
            
            path = Path.Combine(path, saveName);
            if (!File.Exists(path)) return;
            
            managerData = Load.LoadData(path) as float[];

            for (int i = 0; i < managerData.Length; i++)
            {
                managers[i].SavedResourceValue = managerData[i];
            }
        }
    }
}