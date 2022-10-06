using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueprintOnMouse : MonoBehaviour
{
    private Vector3 mousePos;
    private Vector3 spawnPos;
    private Camera mainCam;


    private void Start()
    {
        mainCam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = Input.mousePosition;
        spawnPos = mainCam.ScreenToWorldPoint(mousePos);

        transform.position = spawnPos;
    }
}
