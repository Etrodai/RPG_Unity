using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField] private InputAction cameraMovement;
    [SerializeField] private InputAction cameraRotation;
    [SerializeField] private InputAction cameraZoom;
    [SerializeField] private float moveSpeed;
    [SerializeField] private Vector2 minMaxHeight;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float zoomSpeed;
    [SerializeField] private Vector2 minMaxZoom;
    private float moveDirection;
    private float rotation;
    private float zoom;
    private Transform transform;
    private Transform camera;

    private void Awake()
    {
        transform = gameObject.transform;
        camera = GetComponentInChildren<Camera>().transform;
        zoomSpeed *= 0.008333333f;
        minMaxZoom *= -1;
    }

    private void OnEnable()
    {
        cameraMovement.Enable();
        cameraRotation.Enable();
        cameraZoom.Enable();
    }

    private void OnDisable()
    {
        cameraMovement.Disable();
        cameraRotation.Disable();
        cameraZoom.Disable();
    }

    private void Update()
    {
        moveDirection = cameraMovement.ReadValue<float>() * moveSpeed;
        rotation = cameraRotation.ReadValue<float>() * rotationSpeed;
        zoom = cameraZoom.ReadValue<float>() * zoomSpeed;
    }

    private void FixedUpdate()
    {
        transform.position += new Vector3(0, moveDirection, 0);
        var position = transform.position;
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
        
        transform.Rotate(0,  rotation, 0);
        
        camera.localPosition += new Vector3(0, 0, zoom);
        var localPosition = camera.localPosition;
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
}
