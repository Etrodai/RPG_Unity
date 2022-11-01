using System;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Events;

namespace SaveSystem
{
    public static class Save
    {
        public static readonly UnityEvent OnSaveButtonClick = new UnityEvent();
        public static readonly UnityEvent<string> OnSaveAsButtonClick = new UnityEvent<string>();
        
        public static void AutoSaveData(IEnumerable data, string name)
        {
            if (!Directory.Exists(Path.Combine(Application.persistentDataPath, $@"Data\\Autosafe")))
            {
                Directory.CreateDirectory(Path.Combine(Application.persistentDataPath, $@"Data\\Autosafe"));
            }
            string path = Path.Combine(Application.persistentDataPath, $@"Data\\Autosafe\\{name}.dat");
            SaveData(data, path);
        }
        
        // public static void AutoSaveData(float data, string name)
        // {
        //     if (!Directory.Exists(Path.Combine(Application.persistentDataPath, $@"Data\\Autosafe")))
        //     {
        //         Directory.CreateDirectory(Path.Combine(Application.persistentDataPath, $@"Data\\Autosafe"));
        //     }
        //     string path = Path.Combine(Application.persistentDataPath, $@"Data\\Autosafe\\{name}.dat");
        //     SaveData(data, path);
        // }

        public static void SaveDataAs(string savePlace, IEnumerable data, string name)
        {
            if (!Directory.Exists(Path.Combine(Application.persistentDataPath, $@"Data\\{savePlace}")))
            {
                Directory.CreateDirectory(Path.Combine(Application.persistentDataPath, $@"Data\\{savePlace}"));
            }
            string path = Path.Combine(Application.persistentDataPath, $"@Data\\{savePlace}\\{name}.dat");
            SaveData(data, path);
        }
        
        // public static void SaveDataAs(string savePlace, float data, string name)
        // {
        //     if (!Directory.Exists(Path.Combine(Application.persistentDataPath, $@"Data\\{savePlace}")))
        //     {
        //         Directory.CreateDirectory(Path.Combine(Application.persistentDataPath, $@"Data\\{savePlace}"));
        //     }
        //     string path = Path.Combine(Application.persistentDataPath, $"@Data\\{savePlace}\\{name}.dat");
        //     SaveData(data, path);
        // }

        private static void SaveData(IEnumerable data, string path)
        {
            try
            {
                FileStream fs = File.Create(path);
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(fs, data);
                fs.Close();
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
        
        private static void SaveData(float data, string path)
        {
            try
            {
                FileStream fs = File.Create(path);
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(fs, data);
                fs.Close();
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        private static void SaveAll()
        {
            OnSaveButtonClick?.Invoke();
        }

        private static void SaveAllAs(string savePlace)
        {
            OnSaveAsButtonClick?.Invoke(savePlace);
        }
    }
}
