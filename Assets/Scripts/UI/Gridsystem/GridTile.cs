using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GridTile : MonoBehaviour
{
    [SerializeField] private GameObject prefabGridTileAvailable;
    [SerializeField] private GameObject prefabGridTileLocked;

    private MeshRenderer meshRenderer;
    private MeshFilter meshFilter;


    private bool isLocked;

    public bool IsLocked
    {
        get => isLocked;
        set => isLocked = value;
    }


    /// <summary>
    /// Collect Components
    /// </summary>
    private void Awake()
    {
        meshRenderer = this.GetComponent<MeshRenderer>();
        meshFilter = this.GetComponent<MeshFilter>();

        meshRenderer.enabled = false;
        int layerIgnoreRayCast = LayerMask.NameToLayer("Ignore Raycast");
        this.gameObject.layer = layerIgnoreRayCast;
    }

    /// <summary>
    /// Get Init Materials
    /// </summary>
    private void Start()
    {
        meshFilter.sharedMesh = prefabGridTileAvailable.GetComponent<MeshFilter>().sharedMesh;
        meshRenderer.sharedMaterial = prefabGridTileAvailable.GetComponent<MeshRenderer>().sharedMaterial;
    }

    private void OnTriggerEnter(Collider other)
    {
        IsLocked = true;
        meshRenderer.sharedMaterial = prefabGridTileLocked.GetComponent<MeshRenderer>().sharedMaterial;
    }

    private void OnTriggerExit(Collider other)
    {
        IsLocked = false;
        meshRenderer.sharedMaterial = prefabGridTileAvailable.GetComponent<MeshRenderer>().sharedMaterial;
    }

    /// <summary>
    /// Setting Renderer and Collider active or inactive
    /// </summary>
    /// <param name="isVisible">enabler</param>
    public void ChangeActiveState(bool isVisible)
    {
        meshRenderer.enabled = isVisible;

        if (!meshRenderer.enabled)
        {
            int layerIgnoreRayCast = LayerMask.NameToLayer("Ignore Raycast");
            this.gameObject.layer = layerIgnoreRayCast;
        }
        else
        {
            this.gameObject.layer = 0; //Default Layer
        }
    }


    /// <summary>
    /// Activate Ignore Raycast by Showing Grid
    /// </summary>
    public void InitActiveState()
    {
        meshRenderer.enabled = true;
        int layerIgnoreRayCast = LayerMask.NameToLayer("Ignore Raycast");
        this.gameObject.layer = layerIgnoreRayCast;
    }

    
}