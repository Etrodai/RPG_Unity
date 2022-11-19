using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Events;

namespace SaveSystem
{
    public static class Load
    {
        public static readonly UnityEvent<GameSave> OnLoadButtonClick = new();

        public static GameSave LoadData(string path)
        {
            path = $"{path}.dat";
            if (!File.Exists(path)) return null;
            
            try
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream fs = new FileStream(path, FileMode.Open);
                GameSave data = bf.Deserialize(fs) as GameSave;
                fs.Close();
                return data;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                throw;
            }
        }
    }
}
