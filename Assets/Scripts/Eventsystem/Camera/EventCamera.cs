using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EventCamera : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 100;
    private void Update()
    {
        this.transform.Rotate(0,rotationSpeed * Time.deltaTime,0,Space.Self);
    }
}
