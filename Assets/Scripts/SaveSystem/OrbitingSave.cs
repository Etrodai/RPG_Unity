using System.Collections.Generic;
using System.IO;
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

        private const string SaveName = "PlanetOrbiting";
        public static List<GameObject> Planets { get; } = new();

        #endregion

        #region UnityEvents

        private void Start()
        {
            Save.OnSaveButtonClick.AddListener(SaveData);
            Save.OnSaveAsButtonClick.AddListener(SaveDataAs);
            Load.OnLoadButtonClick.AddListener(LoadData);
        }

        #endregion

        #region Save & Load

        private void SaveData()
        {
            OrbitingData[] position = new OrbitingData[Planets.Count];
            for (int i = 0; i < Planets.Count; i++)
            {
                position[i].x = Planets[i].transform.position.x;
                position[i].y = Planets[i].transform.position.y;
                position[i].z = Planets[i].transform.position.z;
            }

            Save.AutoSaveData(position, SaveName);
        }

        private void SaveDataAs(string savePlace)
        {
            OrbitingData[] position = new OrbitingData[Planets.Count];
            for (int i = 0; i < Planets.Count; i++)
            {
                position[i].x = Planets[i].transform.position.x;
                position[i].y = Planets[i].transform.position.y;
                position[i].z = Planets[i].transform.position.z;
            }

            Save.SaveDataAs(savePlace, position, SaveName);
        }

        private void LoadData(string path)
        {
            path = Path.Combine(path, $"{SaveName}.dat");
            if (!File.Exists(path)) return;
            
            OrbitingData[] position = Load.LoadData(path) as OrbitingData[];

            // if (position == null) return;
            
            for (int i = 0; i < Planets.Count; i++)
            {
                Planets[i].transform.position = new Vector3(position[i].x, position[i].y, position[i].z);
            }
        }

        #endregion
    }
}