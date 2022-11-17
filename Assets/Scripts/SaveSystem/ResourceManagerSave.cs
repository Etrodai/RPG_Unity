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
        private SaveData saveData;

        private void Start()
        {
            saveData = SaveSystem.SaveData.Instance;
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

        private void SaveData()
        {
            managers = new ResourceManager[5];
            managers[0] = MainManagerSingleton.Instance.CitizenManager;
            managers[1] = MainManagerSingleton.Instance.EnergyManager;
            managers[2] = MainManagerSingleton.Instance.FoodManager;
            managers[3] = MainManagerSingleton.Instance.MaterialManager;
            managers[4] = MainManagerSingleton.Instance.WaterManager;
            managerData = new float[5];
            for (int i = 0; i < managers.Length; i++)
            {
                managerData[i] = managers[i].SavedResourceValue;
            }

            saveData.GameSave.managerData = managerData;
            // Save.AutoSaveData(managerData, SaveName);
        }
        
        private void SaveDataAs(string savePlace)
        {
            managers = new ResourceManager[5];
            managers[0] = MainManagerSingleton.Instance.CitizenManager;
            managers[1] = MainManagerSingleton.Instance.EnergyManager;
            managers[2] = MainManagerSingleton.Instance.FoodManager;
            managers[3] = MainManagerSingleton.Instance.MaterialManager;
            managers[4] = MainManagerSingleton.Instance.WaterManager;
            managerData = new float[5];
            for (int i = 0; i < managers.Length; i++)
            {
                managerData[i] = managers[i].SavedResourceValue;
            }

            saveData.GameSave.managerData = managerData;
            // Save.SaveDataAs(savePlace, managerData, SaveName);
        }

        private void LoadData(GameSave gameSave)
        {
            managers = new ResourceManager[5];
            managers[0] = MainManagerSingleton.Instance.CitizenManager;
            managers[1] = MainManagerSingleton.Instance.EnergyManager;
            managers[2] = MainManagerSingleton.Instance.FoodManager;
            managers[3] = MainManagerSingleton.Instance.MaterialManager;
            managers[4] = MainManagerSingleton.Instance.WaterManager;

            managerData = gameSave.managerData;

            for (int i = 0; i < managerData.Length; i++)
            {
                managers[i].SavedResourceValue = managerData[i];
            }
        }
    }
}