using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PirateRotation : MonoBehaviour //Made by Eric
{
    [SerializeField] private float rotationSpeed;
    private float yRotation;

    private void OnEnable()
    {
        yRotation = 0f;
    }

    private void Update()
    {
        yRotation += Time.unscaledDeltaTime * rotationSpeed;
        transform.rotation = Quaternion.Euler(0f, yRotation, 0);
    }
}
