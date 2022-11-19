using UnityEngine;

namespace Eventsystem.Camera
{
    public class EventCamera : MonoBehaviour
    {
        [SerializeField] private float rotationSpeed = 100;
        private void Update()
        {
            transform.Rotate(0,rotationSpeed * Time.deltaTime,0,Space.Self);
        }
    }
}
