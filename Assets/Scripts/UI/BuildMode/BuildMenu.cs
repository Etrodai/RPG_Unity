using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UI.BuildMode
{
    public class BuildMenu : MonoBehaviour
    {
        [SerializeField] private GameObject buildMenuLayout;
        private Camera mainCam;

        [SerializeField] private GameObject prefabBlueprintObject;
        private GameObject blueprintObject;
        private GameObject moduleToBuild;

        private Vector3 mousePos;
        
        //Input
        [SerializeField] private PlayerInput playerInput;
        private bool playerInputHasBeenInit;

        private void Start()
        {
            mainCam = Camera.main;
        }

        private void Update()
        {
            if (!playerInputHasBeenInit)
            {
                InitPlayerInput();
            }
        }

        private void OnDisable()
        {
            if (playerInput == null) return;
            playerInput.actions["LeftClick"].performed -= LeftMouseButtonPressed;
            playerInput.actions["OpenBuildMenu"].performed -= RightMouseButtonPressed;
            playerInputHasBeenInit = false;
        }

        private void InitPlayerInput()
        {
            playerInput.actions["LeftClick"].performed += LeftMouseButtonPressed;
            playerInput.actions["OpenBuildMenu"].performed += RightMouseButtonPressed;
            playerInputHasBeenInit = true;
        }

        /// <summary>
        /// De/Activate Build Menu
        /// </summary>
        private void RightMouseButtonPressed(InputAction.CallbackContext context)
        {
            CheckIfBlueprintObjectExists();
            if (buildMenuLayout == null) return;
            buildMenuLayout.SetActive(!buildMenuLayout.activeSelf);

            if (buildMenuLayout.activeSelf)
            {
                mousePos = Input.mousePosition;
                buildMenuLayout.transform.position = mousePos;
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

        private void LeftMouseButtonPressed(InputAction.CallbackContext context)
        {
            
            if (buildMenuLayout == null || !buildMenuLayout.activeSelf)
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
                        blueprint.gridTileHit.Module = module;
                    }
                }

                buildMenuLayout.SetActive(false);
            }

            CheckIfBlueprintObjectExists();
            UnCheckAvailableGridTiles(true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="moduleToBuildGameObject"></param>
        public void BuildMenuButtonPressed(GameObject moduleToBuildGameObject)
        {
            mousePos = Input.mousePosition;
            Vector3 spawnPos = mainCam.ScreenToWorldPoint(mousePos);
            CheckAvailableGridTiles();

            this.moduleToBuild = moduleToBuildGameObject;
            blueprintObject = Instantiate(prefabBlueprintObject, spawnPos, Quaternion.identity);
        }

        private void CheckAvailableGridTiles()
        {
            Gridsystem.Gridsystem.Instance.CheckAvailableGridTilesAroundStation();
        }

        private void UnCheckAvailableGridTiles(bool isBuilt)
        {
            Gridsystem.Gridsystem.Instance.UnCheckAvailableGridTilesAroundStation(isBuilt);
        }
    }
}