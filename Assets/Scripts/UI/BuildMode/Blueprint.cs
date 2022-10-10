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
    public bool IsLockedIn { get => isLockedIn; set => isLockedIn = value; }
    
    private Ray mouseRay;
    private float rayRange = 100;



    private void Awake()
    {
        mainCam = Camera.main;
        station = GameObject.FindGameObjectWithTag("Station").transform; //Need Rework
    }

    private void LateUpdate()
    {
        distance = Vector3.Distance(mainCam.transform.position, station.position);
        mousePos = Input.mousePosition;
        posInWorld = mainCam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, distance));
        
        mouseRay = mainCam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(mouseRay, out RaycastHit hit, rayRange))
        {
            this.transform.position = hit.transform.position;
            IsLockedIn = true;
        }
        else
        {
            this.transform.position = posInWorld;
            IsLockedIn = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //isLockedIn = true;
        //this.transform.position = other.transform.position;
    }

    private void OnTriggerExit(Collider other)
    {
    }
}