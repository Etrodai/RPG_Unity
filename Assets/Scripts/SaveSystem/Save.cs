using System;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.Events;

namespace SaveSystem
{
    public static class Save
    {
        public static readonly UnityEvent OnSaveButtonClick = new();
        public static readonly UnityEvent<string> OnSaveAsButtonClick = new();
        
        public static void AutoSaveData(IEnumerable data, string name)
        {
            //C:/Users/robin/AppData/LocalLow/DefaultCompany/RPG_Unity
            if (!Directory.Exists(Path.Combine(Application.persistentDataPath, $@"Data\\Autosafe")))
            {
                Directory.CreateDirectory(Path.Combine(Application.persistentDataPath, $@"Data\\Autosafe"));
            }
            string path = Path.Combine(Application.persistentDataPath, $@"Data\\Autosafe\\{name}.dat");
            SaveData(data, path);
            // SaveData(data, path, true);
            // SaveData(data, path, false);
        }

        public static void SaveDataAs(string savePlace, IEnumerable data, string name)
        {
            if (!Directory.Exists(Path.Combine(Application.persistentDataPath, $@"Data\\{savePlace}")))
            {
                Directory.CreateDirectory(Path.Combine(Application.persistentDataPath, $@"Data\\{savePlace}"));
            }
            string path = Path.Combine(Application.persistentDataPath, $@"Data\\{savePlace}\\{name}.dat");
            SaveData(data, path);
            // SaveData(data, path, true);
            // SaveData(data, path, false);
        }

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

        private static void SaveData(IEnumerable data, string path, bool useXml1OrJson0)
        {
            if (useXml1OrJson0)
            {
                try
                {
                    FileStream fs = File.Create(path);
                    XmlSerializer xs = new XmlSerializer(typeof(IEnumerable));
                    xs.Serialize(fs, data);
                    fs.Close();
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }
            }
            else
            {
                try
                {
                    string json = JsonUtility.ToJson(data);
                    File.WriteAllText(path, json);

                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }
            }
        }
    }
}
