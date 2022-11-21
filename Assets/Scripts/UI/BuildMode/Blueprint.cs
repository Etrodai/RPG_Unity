using UI.Gridsystem;
using UnityEngine;

namespace UI.BuildMode
{
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

        private MeshRenderer _meshRenderer;
        private MeshFilter _meshFilter;
        public bool IsLockedIn { get; private set; }

        private Ray mouseRay;
        private const float RayRange = 100;

        public GridTile gridTileHit;

        private void Awake()
        {
            mainCam = Camera.main;
            station = GameObject.FindGameObjectWithTag("Station").transform; //TODO: (Ben) Need Rework
        }

        private void Update()
        {
            distance = Vector3.Distance(mainCam.transform.position, station.position);
            mousePos = Input.mousePosition;
            posInWorld = mainCam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, distance));

            mouseRay = mainCam.ScreenPointToRay(mousePos);
            if (Physics.Raycast(mouseRay, out RaycastHit hit, RayRange))
            {
                gridTileHit = hit.transform.gameObject.GetComponent<GridTile>(); //TODO: (Ben) VERY Expensive
                this.transform.position = hit.transform.position;
                IsLockedIn = true;
            }
            else
            {
                this.transform.position = posInWorld;
                IsLockedIn = false;
            }
        }
    }
}