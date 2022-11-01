using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using SaveSystem;
using UnityEngine;

public class OrbitingSave : MonoBehaviour
{
    public static List<GameObject> Planets { get; set; }

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
        
        Save.AutoSaveData(position, "PlanetOrbiting");
    }
    
    private void SaveDataAs(string savePlace)
    {
        Vector3[] position = new Vector3[Planets.Count];
        for (int i = 0; i < Planets.Count; i++)
        {
            position[i] = Planets[i].transform.position;
        }
        
        Save.SaveDataAs(savePlace, position, "PlanetOrbiting");
    }
    
    private void LoadData(string path)
    {
        path = Path.Combine(path, "PlanetOrbiting");

        Vector3[] position = Load.LoadData(path);
        
        for (int i = 0; i < Planets.Count; i++)
        {
            Planets[i].transform.position = position[i];
        }
    }
}
