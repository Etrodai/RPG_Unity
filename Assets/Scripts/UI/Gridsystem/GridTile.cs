using UnityEngine;

namespace UI.Gridsystem
{
    public class GridTile : MonoBehaviour
    {
        [SerializeField] private GameObject prefabGridTileAvailable;
        [SerializeField] private GameObject prefabGridTileLocked;

        public GameObject Module { get; set; }

        private MeshRenderer meshRenderer;
        private MeshFilter meshFilter;


        public bool IsLocked { get; set; }

        public bool HasModule { get; set; }


        /// <summary>
        /// Collect Components
        /// </summary>
        private void Awake()
        {
            meshRenderer = this.GetComponent<MeshRenderer>();
            meshFilter = this.GetComponent<MeshFilter>();

            meshRenderer.enabled = false;
            int layerIgnoreRayCast = LayerMask.NameToLayer("Ignore Raycast");
            this.gameObject.layer = layerIgnoreRayCast;
        }

        /// <summary>
        /// Get Init Materials
        /// </summary>
        private void Start()
        {
            meshFilter.sharedMesh = prefabGridTileAvailable.GetComponent<MeshFilter>().sharedMesh;
            meshRenderer.sharedMaterial = prefabGridTileAvailable.GetComponent<MeshRenderer>().sharedMaterial;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!meshRenderer.isVisible)
                return;

            IsLocked = true;
            meshRenderer.sharedMaterial = prefabGridTileLocked.GetComponent<MeshRenderer>().sharedMaterial;
        }

        private void OnTriggerExit(Collider other)
        {
            if (!meshRenderer.isVisible)
                return;

            IsLocked = false;
            meshRenderer.sharedMaterial = prefabGridTileAvailable.GetComponent<MeshRenderer>().sharedMaterial;
        }

        /// <summary>
        /// Setting Renderer and Collider active or inactive
        /// </summary>
        /// <param name="isVisible">enabler</param>
        public void ChangeActiveState(bool isVisible)
        {
            meshRenderer.enabled = isVisible;

            if (!meshRenderer.enabled)
            {
                int layerIgnoreRayCast = LayerMask.NameToLayer("Ignore Raycast");
                this.gameObject.layer = layerIgnoreRayCast;
                meshRenderer.sharedMaterial = prefabGridTileAvailable.GetComponent<MeshRenderer>().sharedMaterial;
            }
            else
            {
                this.gameObject.layer = 0; //Default Layer
            }
        }

        public bool SetModuleOnUsed()
        {
            HasModule = true;
            if (HasModule)
                return HasModule;
            else
            {
                Debug.Log("Error: No Module could be set");
                return false;
            }
        }

        /// <summary>
        /// Activate Ignore Raycast by Showing Grid
        /// </summary>
        public void InitActiveState()
        {
            meshRenderer.enabled = true;
            int layerIgnoreRayCast = LayerMask.NameToLayer("Ignore Raycast");
            this.gameObject.layer = layerIgnoreRayCast;
        }
    }
}