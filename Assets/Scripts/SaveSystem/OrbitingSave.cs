using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using SaveSystem;
using UnityEngine;

public class OrbitingSave : MonoBehaviour
{
    public static List<GameObject> Planets { get; } = new();
    private const string saveName = "PlanetOrbiting";

    private void Start()
    {
        Save.OnSaveButtonClick.AddListener(SaveData);
        Save.OnSaveAsButtonClick.AddListener(SaveDataAs);
        Load.OnLoadButtonClick.AddListener(LoadData);
    }

    private void SaveData()
    {
        Vector3[] position = new Vector3[Planets.Count];
        for (int i = 0; i < Planets.Count; i++)
        {
            position[i] = Planets[i].transform.position;
        }
        
        Save.AutoSaveData(position, saveName);
    }
    
    private void SaveDataAs(string savePlace)
    {
        Vector3[] position = new Vector3[Planets.Count];
        for (int i = 0; i < Planets.Count; i++)
        {
            position[i] = Planets[i].transform.position;
        }
        
        Save.SaveDataAs(savePlace, position, saveName);
    }
    
    private void LoadData(string path)
    {
        path = Path.Combine(path, saveName);

        Vector3[] position = Load.LoadData(path) as Vector3[];
        
        for (int i = 0; i < Planets.Count; i++)
        {
            Planets[i].transform.position = position[i];
        }
    }
}
