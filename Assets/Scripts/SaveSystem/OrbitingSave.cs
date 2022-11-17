using System;
using System.Collections.Generic;
using UnityEngine;

namespace SaveSystem
{
    [System.Serializable]
    public struct OrbitingData
    {
        public float x;
        public float y;
        public float z;
    }
    
    public class OrbitingSave : MonoBehaviour
    {
        #region Variables & Properties
        
        public static List<GameObject> Planets { get; } = new();

        #endregion

        #region UnityEvents

        private void Start()
        {
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

        private void SaveData(SaveLoadInvoker invoker)
        {
            OrbitingData[] position = new OrbitingData[Planets.Count];
            for (int i = 0; i < Planets.Count; i++)
            {
                position[i].x = Planets[i].transform.position.x;
                position[i].y = Planets[i].transform.position.y;
                position[i].z = Planets[i].transform.position.z;
            }

            invoker.GameSave.orbitingData = position;
        }

        private void SaveDataAs(string savePlace, SaveLoadInvoker invoker)
        {
            OrbitingData[] position = new OrbitingData[Planets.Count];
            for (int i = 0; i < Planets.Count; i++)
            {
                position[i].x = Planets[i].transform.position.x;
                position[i].y = Planets[i].transform.position.y;
                position[i].z = Planets[i].transform.position.z;
            }

            invoker.GameSave.orbitingData = position;
        }

        private void LoadData(GameSave gameSave)
        {
            OrbitingData[] position = gameSave.orbitingData;

            for (int i = 0; i < Planets.Count; i++)
            {
                Planets[i].transform.position = new Vector3(position[i].x, position[i].y, position[i].z);
            }
        }

        #endregion
    }
}