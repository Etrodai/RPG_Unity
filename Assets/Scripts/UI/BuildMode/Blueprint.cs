using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script for Handling Module before Placement
/// </summary>
public class Blueprint : MonoBehaviour
{
    //Mouse and Positions
    private Vector3 mousePos;
    private Vector3 posInWorld;
    private float distance;

    private Camera mainCam;
    private Transform station;

    private bool isLockedIn;

    public bool IsLockedIn
    {
        get => isLockedIn;
        set => isLockedIn = value;
    }

    private Ray mouseRay;
    private float rayRange = 100;

    public GridTile gridTileHit;

    private void Awake()
    {
        mainCam = Camera.main;
        station = GameObject.FindGameObjectWithTag("Station").transform; //Need Rework
    }

    private void Update()
    {
        distance = Vector3.Distance(mainCam.transform.position, station.position);
        mousePos = Input.mousePosition;
        posInWorld = mainCam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, distance));

        mouseRay = mainCam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(mouseRay, out RaycastHit hit, rayRange))
        {
            if (gridTileHit == null)
                gridTileHit = hit.transform.gameObject.GetComponent<GridTile>(); //VERY Expensive
            this.transform.position = hit.transform.position;
            IsLockedIn = true;
        }
        else
        {
            if (gridTileHit != null)
                gridTileHit = null;

            this.transform.position = posInWorld;
            IsLockedIn = false;
        }
    }
}