using Unity.Mathematics;
using UnityEngine;

namespace UI.BuildMode
{
    public class BuildMenu : MonoBehaviour
    {
        [SerializeField] private GameObject buildmenuLayout;
        private Camera mainCam;

        [SerializeField] private GameObject prefab_blueprintObject;
        private GameObject blueprintObject;
        private GameObject moduleToBuild;

        Vector3 mousePos;

        private void Start()
        {
            mainCam = Camera.main;
        }

        /// <summary>
        /// De/Activate Build Menu
        /// </summary>
        public void RightMouseButtonPressed()
        {
            CheckIfBlueprintObjectExists();
            buildmenuLayout.SetActive(!buildmenuLayout.activeSelf);

            if (buildmenuLayout.activeSelf)
            {
                mousePos = Input.mousePosition;
                buildmenuLayout.transform.position = mousePos;
            }
            else
            {
                Destroy(blueprintObject);
                UnCheckAvailableGridTiles(false);
            }
        }

        private void CheckIfBlueprintObjectExists()
        {
            if (blueprintObject != null)
            {
                Destroy(blueprintObject);
            }
        }

        public void LeftMouseButtonPressed()
        {
            if (!buildmenuLayout.activeSelf)
                return;


            if (blueprintObject != null)
            {
                Blueprint blueprint = blueprintObject.GetComponent<Blueprint>();

                if (blueprint.IsLockedIn)
                {
                    if (blueprint.gridTileHit.SetModuleOnUsed())
                    {
                        // Debug.Log(blueprint.gridTileHit.name);
                        GameObject module = Instantiate(moduleToBuild, blueprint.transform.position, quaternion.identity);
                        module.transform.parent = GameObject.FindGameObjectWithTag("Station").transform; //TODO: (Ben) Redo
                        blueprint.gridTileHit.GetComponent<Collider>().isTrigger = false;
                    }
                }

                buildmenuLayout.SetActive(false);
            }

            CheckIfBlueprintObjectExists();
            UnCheckAvailableGridTiles(true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objectToBuild"></param>
        public void BuildMenuButtonPressed(GameObject _moduleToBuild)
        {
            mousePos = Input.mousePosition;
            Vector3 spawnPos = mainCam.ScreenToWorldPoint(mousePos);
            CheckAvailableGridTiles();

            moduleToBuild = _moduleToBuild;
            blueprintObject = Instantiate(prefab_blueprintObject, spawnPos, Quaternion.identity);
        }

        private void CheckAvailableGridTiles()
        {
            Gridsystem.Gridsystem.Instance.CheckAvailableGridTilesAroundStation();
        }

        private void UnCheckAvailableGridTiles(bool isBuilded)
        {
            Gridsystem.Gridsystem.Instance.UnCheckAvailableGridTilesAroundStation(isBuilded);
        }
    }
}