using UnityEngine;

namespace SolarSystem
{
    /// <summary>
    /// Orbiting around a angle Point
    ///
    /// Code by:  https://www.youtube.com/watch?v=En7iugIoG_A
    /// </summary>
    public class Orbiting : MonoBehaviour
    {
        public float xSpread;
        public float zSpread;
    
        public float yOffset;
        public float timeOffset = 0.2f;
    
        public Transform centerPoint;

        public float rotSpeed;
        public bool rotateClockwise;

        private float timer = 0;

        private void Update()
        {
            timer += Time.deltaTime * rotSpeed;
            Rotate();
        }

        private void Rotate()
        {
            if (rotateClockwise)
            {
                float x = -Mathf.Cos(timer) * xSpread;
                float z = Mathf.Sin(timer) * zSpread;
                float y = Mathf.Cos(timer + timeOffset) * yOffset;
                Vector3 pos = new Vector3(x, y, z);
                transform.position = pos + centerPoint.position;
            }
            else
            {
                float x = Mathf.Cos(timer) * xSpread;
                float z = Mathf.Sin(timer) * zSpread;
                float y = Mathf.Cos(timer + timeOffset) * yOffset;
                Vector3 pos = new Vector3(x, y, z);
                transform.position = pos + centerPoint.position;
            }
        }
    }
}
