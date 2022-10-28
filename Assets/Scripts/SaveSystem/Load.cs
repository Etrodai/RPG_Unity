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
        public static UnityEvent<string> onLoadButtonClick = new();

        public static IEnumerable LoadIEnumerableData(string path)
        {
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
        public static float LoadFloatData(string path)
        {
            if (File.Exists(path))
            {
                try
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    FileStream fs = new FileStream(path, FileMode.Open);
                    float data = bf.Deserialize(fs) is float ? (float) bf.Deserialize(fs) : 0;
                    fs.Close();
                    return data;
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                    throw;
                }
            }
            return 0;
        }
    }
}
