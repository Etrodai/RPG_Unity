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
    private BoxCollider boxCollider;

    private bool isLocked = false;

    /// <summary>
    /// Collect Components
    /// </summary>
    private void Awake()
    {
        meshRenderer = this.GetComponent<MeshRenderer>();
        meshFilter = this.GetComponent<MeshFilter>();
        boxCollider = this.GetComponent<BoxCollider>();
    }

    /// <summary>
    /// Get Init Materials
    /// </summary>
    private void Start()
    {
        meshFilter.sharedMesh = prefabGridTileAvailable.GetComponent<MeshFilter>().sharedMesh;
        meshRenderer.sharedMaterial = prefabGridTileAvailable.GetComponent<MeshRenderer>().sharedMaterial;
    }

    private void LateUpdate()
    {
        switch (isLocked)
        {
            case true:
                meshRenderer.sharedMaterial = prefabGridTileLocked.GetComponent<MeshRenderer>().sharedMaterial;
                break;
            case false:
                meshRenderer.sharedMaterial = prefabGridTileAvailable.GetComponent<MeshRenderer>().sharedMaterial;
                break;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        isLocked = true;
    }

    private void OnCollisionExit(Collision other)
    {
        isLocked = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        isLocked = true;
    }

    private void OnTriggerExit(Collider other)
    {
        isLocked = false;
    }

    /// <summary>
    /// Setting Renderer and Collider active or inactive
    /// </summary>
    /// <param name="isVisible">enabler</param>
    public void SetActive(bool isVisible)
    {
        meshRenderer.enabled = isVisible;
        boxCollider.enabled = isVisible;
    }
    
    
    
    /// <summary>
    /// NOT USED
    /// Create GridTiles around object on Scriptstart
    /// </summary>
    private void CreateGridTilesAround()
    {
        //Layer 1
        Vector3 offset = this.transform.position;
        
        offset.x += 1;
        Instantiate(prefabGridTileAvailable, offset, Quaternion.identity);

        offset.z += 1;
        Instantiate(prefabGridTileAvailable, offset, Quaternion.identity);

        offset.z -= 2;
        Instantiate(prefabGridTileAvailable, offset, Quaternion.identity);

        
        offset.y += 1;
        Instantiate(prefabGridTileAvailable, offset, Quaternion.identity);

        offset.z += 1;
        Instantiate(prefabGridTileAvailable, offset, Quaternion.identity);

        offset.z += 1;
        Instantiate(prefabGridTileAvailable, offset, Quaternion.identity);


        offset.y -= 2;
        Instantiate(prefabGridTileAvailable, offset, Quaternion.identity);

        offset.z -= 1;
        Instantiate(prefabGridTileAvailable, offset, Quaternion.identity);

        offset.z -= 1;
        Instantiate(prefabGridTileAvailable, offset, Quaternion.identity);
        

        //Layer 2
        offset = this.transform.position;
        
        offset.z += 1;
        Instantiate(prefabGridTileAvailable, offset, Quaternion.identity);

        offset.z -= 2;
        Instantiate(prefabGridTileAvailable, offset, Quaternion.identity);

        
        offset.y += 1;
        Instantiate(prefabGridTileAvailable, offset, Quaternion.identity);

        offset.z += 1;
        Instantiate(prefabGridTileAvailable, offset, Quaternion.identity);

        offset.z += 1;
        Instantiate(prefabGridTileAvailable, offset, Quaternion.identity);


        offset.y -= 2;
        Instantiate(prefabGridTileAvailable, offset, Quaternion.identity);

        offset.z -= 1;
        Instantiate(prefabGridTileAvailable, offset, Quaternion.identity);

        offset.z -= 1;
        Instantiate(prefabGridTileAvailable, offset, Quaternion.identity);
        
        
        //Layer 3
        
        offset = this.transform.position;
        
        offset.x += -1;
        Instantiate(prefabGridTileAvailable, offset, Quaternion.identity);

        offset.z += 1;
        Instantiate(prefabGridTileAvailable, offset, Quaternion.identity);

        offset.z -= 2;
        Instantiate(prefabGridTileAvailable, offset, Quaternion.identity);

        
        offset.y += 1;
        Instantiate(prefabGridTileAvailable, offset, Quaternion.identity);

        offset.z += 1;
        Instantiate(prefabGridTileAvailable, offset, Quaternion.identity);

        offset.z += 1;
        Instantiate(prefabGridTileAvailable, offset, Quaternion.identity);


        offset.y -= 2;
        Instantiate(prefabGridTileAvailable, offset, Quaternion.identity);

        offset.z -= 1;
        Instantiate(prefabGridTileAvailable, offset, Quaternion.identity);

        offset.z -= 1;
        Instantiate(prefabGridTileAvailable, offset, Quaternion.identity);
    }
}
