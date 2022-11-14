using System;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Events;

namespace SaveSystem
{
    public static class Load
    {
        public static readonly UnityEvent<string> OnLoadButtonClick = new();

        public static IEnumerable LoadData(string path)
        {
            path = $"{path}.dat";
            if (File.Exists(path))
            {
                try
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    FileStream fs = new FileStream(path, FileMode.Open);
                    IEnumerable data = bf.Deserialize(fs) as IEnumerable;
                    fs.Close();
                    return data;
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                    throw;
                }
            }
            return null;
        }
    }
}
