using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Building", menuName = "Buildings")]
public class BuildingResourcesScriptableObject : ScriptableObject
{
    [SerializeField] private Resource[] costs;
    [SerializeField] private Vector3 requiredSpace;
    [SerializeField] private Resource[] consumption;
    [SerializeField] private Resource[] production;
    [SerializeField] private Resource[] saveSpace;

    public Resource[] Costs => costs;
    public Vector3 RequiredSpace => requiredSpace;
    public Resource[] Consumption => consumption;
    public Resource[] Production => production;
    public Resource[] SaveSpace => saveSpace;
}