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
        
        private void SaveData(SaveLoadInvoker invoker)
        {
            managerData = new float[5];
            for (int i = 0; i < managers.Length; i++)
            {
                managerData[i] = managers[i].SavedResourceValue;
            }

            invoker.GameSave.managerData = managerData;
            // Save.AutoSaveData(managerData, SaveName);
        }
        
        private void SaveDataAs(string savePlace, SaveLoadInvoker invoker)
        {
            managerData = new float[5];
            for (int i = 0; i < managers.Length; i++)
            {
                managerData[i] = managers[i].SavedResourceValue;
            }

            invoker.GameSave.managerData = managerData;
            // Save.SaveDataAs(savePlace, managerData, SaveName);
        }

        private void LoadData(GameSave gameSave)
        {
            // path = Path.Combine(path, $"{SaveName}.dat");
            // if (!File.Exists(path)) return;
            //
            // managerData = Load.LoadData(path) as float[];

            managerData = gameSave.managerData;

            // if (managerData == null) return;

            for (int i = 0; i < managerData.Length; i++)
            {
                managers[i].SavedResourceValue = managerData[i];
            }
        }
    }
}