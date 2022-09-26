using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridTile : MonoBehaviour
{
    [SerializeField] private GameObject prefabGrid;

    private void Start()
    {
        //Layer 1
        
        Vector3 offset = this.transform.position;
        
        offset.x += 1;
        Instantiate(prefabGrid, offset, Quaternion.identity);

        offset.z += 1;
        Instantiate(prefabGrid, offset, Quaternion.identity);

        offset.z -= 2;
        Instantiate(prefabGrid, offset, Quaternion.identity);

        
        offset.y += 1;
        Instantiate(prefabGrid, offset, Quaternion.identity);

        offset.z += 1;
        Instantiate(prefabGrid, offset, Quaternion.identity);

        offset.z += 1;
        Instantiate(prefabGrid, offset, Quaternion.identity);


        offset.y -= 2;
        Instantiate(prefabGrid, offset, Quaternion.identity);

        offset.z -= 1;
        Instantiate(prefabGrid, offset, Quaternion.identity);

        offset.z -= 1;
        Instantiate(prefabGrid, offset, Quaternion.identity);
        

        //Layer 2
        offset = this.transform.position;
        
        offset.z += 1;
        Instantiate(prefabGrid, offset, Quaternion.identity);

        offset.z -= 2;
        Instantiate(prefabGrid, offset, Quaternion.identity);

        
        offset.y += 1;
        Instantiate(prefabGrid, offset, Quaternion.identity);

        offset.z += 1;
        Instantiate(prefabGrid, offset, Quaternion.identity);

        offset.z += 1;
        Instantiate(prefabGrid, offset, Quaternion.identity);


        offset.y -= 2;
        Instantiate(prefabGrid, offset, Quaternion.identity);

        offset.z -= 1;
        Instantiate(prefabGrid, offset, Quaternion.identity);

        offset.z -= 1;
        Instantiate(prefabGrid, offset, Quaternion.identity);
        
        
        //Layer 3
        
        offset = this.transform.position;
        
        offset.x += -1;
        Instantiate(prefabGrid, offset, Quaternion.identity);

        offset.z += 1;
        Instantiate(prefabGrid, offset, Quaternion.identity);

        offset.z -= 2;
        Instantiate(prefabGrid, offset, Quaternion.identity);

        
        offset.y += 1;
        Instantiate(prefabGrid, offset, Quaternion.identity);

        offset.z += 1;
        Instantiate(prefabGrid, offset, Quaternion.identity);

        offset.z += 1;
        Instantiate(prefabGrid, offset, Quaternion.identity);


        offset.y -= 2;
        Instantiate(prefabGrid, offset, Quaternion.identity);

        offset.z -= 1;
        Instantiate(prefabGrid, offset, Quaternion.identity);

        offset.z -= 1;
        Instantiate(prefabGrid, offset, Quaternion.identity);
    }
}
