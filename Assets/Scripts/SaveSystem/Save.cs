using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.Events;

namespace SaveSystem
{
    public static class Save //Made by Robin
    {
        public static readonly UnityEvent OnSaveButtonClick = new();
        public static readonly UnityEvent<string> OnSaveAsButtonClick = new();
        
        public static void AutoSaveData(GameSave data, string name)
        {
            //C:/Users/robin/AppData/LocalLow/DefaultCompany/RPG_Unity
            if (!Directory.Exists(Path.Combine(Application.persistentDataPath, $@"Data\\Autosafe")))
            {
                Directory.CreateDirectory(Path.Combine(Application.persistentDataPath, $@"Data\\Autosafe"));
            }
            string pathBinary = Path.Combine(Application.persistentDataPath, $@"Data\\Autosafe\\{name}.dat");
            string pathXml = Path.Combine(Application.persistentDataPath, $@"Data\\Autosafe\\{name}.xml");
            string pathJson = Path.Combine(Application.persistentDataPath, $@"Data\\Autosafe\\{name}.json");
            SaveData(data, pathBinary);
            SaveData(data, pathXml, true);
            SaveData(data, pathJson, false);
        }

        public static void SaveDataAs(string savePlace, GameSave data, string name)
        {
            if (!Directory.Exists(Path.Combine(Application.persistentDataPath, $@"Data\\{savePlace}")))
            {
                Directory.CreateDirectory(Path.Combine(Application.persistentDataPath, $@"Data\\{savePlace}"));
            }
            string pathBinary = Path.Combine(Application.persistentDataPath, $@"Data\\{savePlace}\\{name}.dat");
            string pathXml = Path.Combine(Application.persistentDataPath, $@"Data\\{savePlace}\\{name}.xml");
            string pathJson = Path.Combine(Application.persistentDataPath, $@"Data\\{savePlace}\\{name}.json");
            SaveData(data, pathBinary);
            SaveData(data, pathXml, true);
            SaveData(data, pathJson, false);
        }

        private static void SaveData(GameSave data, string path)
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
            }
        }

        private static void SaveData(GameSave data, string path, bool useXml1OrJson0)
        {
            if (useXml1OrJson0)
            {
                try
                {
                    FileStream fs = File.Create(path);
                    XmlSerializer xs = new XmlSerializer(typeof(GameSave));
                    xs.Serialize(fs, data);
                    fs.Close();
                }
                catch (Exception e)
                {
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
                }
            }
        }
    }
}
