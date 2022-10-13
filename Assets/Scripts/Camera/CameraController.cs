using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    #region TODOS
    
    // use Cine machine

    #endregion

    #region Variables
    
    [SerializeField] private float moveSpeed;
    [SerializeField] private Vector2 minMaxHeight;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float zoomSpeed;
    [SerializeField] private Vector2 minMaxZoom;
    private float moveDirection;
    private float rotation;
    private float zoom;
    private new Transform transform;
    private new Transform camera;

    #endregion

    #region UnityEvents

    /// <summary>
    /// sets Variables
    /// </summary>
    private void Awake()
    {
        transform = gameObject.transform;
        camera = GetComponentInChildren<Camera>().transform;
        zoomSpeed *= 0.008333333f;
        minMaxZoom *= -1;
    }

    private void FixedUpdate()
    {
        Move();
        Rotate();
        Zoom();
    }

    #endregion

    #region Methods

    private void Move()
    {
        Vector3 position = transform.position;
        position += new Vector3(0, moveDirection * moveSpeed, 0);
            
        if (position.y < minMaxHeight.x)
        {
            position = new Vector3(position.x, minMaxHeight.x, position.z);
            transform.position = position;
        }
        else if (position.y > minMaxHeight.y)
        {
            position = new Vector3(position.x, minMaxHeight.y, position.z);
            transform.position = position;
        }
    }

    private void Rotate()
    {
        transform.Rotate(0, rotation * rotationSpeed, 0);
    }

    private void Zoom()
    {
        Vector3 localPosition = camera.localPosition;
        localPosition += new Vector3(0, 0, zoom * zoomSpeed);

        if (localPosition.z > minMaxZoom.x)
        {
            localPosition = new Vector3(localPosition.x, localPosition.y, minMaxZoom.x);
            camera.localPosition = localPosition;
        }
        else if (localPosition.z < minMaxZoom.y)
        {
            localPosition = new Vector3(localPosition.x, localPosition.y, minMaxZoom.y);
            camera.localPosition = localPosition;
        }
    }

    #endregion
    
    #region InputActions

    public void Move(InputAction.CallbackContext ctx)
    {
        var check = ctx.ReadValueAsObject();
        switch (check)
        {
            case null:
                moveDirection = 0;
                return;
            case float movementDirection:
                moveDirection = movementDirection;
                break;
        }
    }

    public void Rotate(InputAction.CallbackContext ctx)
    {
        var check = ctx.ReadValueAsObject();
        switch (check)
        {
            case null:
                rotation = 0;
                return;
            case float rotationDirection:
                rotation = rotationDirection;
                break;
        }
    }

    public void Zoom(InputAction.CallbackContext ctx)
    {
        var check = ctx.ReadValueAsObject();
        switch (check)
        {
            case null:
                rotation = 0;
                return;
            case float zoomDirection:
                rotation = zoomDirection;
                break;
        }
    }
    
    #endregion
}