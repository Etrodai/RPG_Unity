using System.Reflection;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UI.BuildMode
{
    /// <summary>
    /// Controlling Behaviour in Build Menu and the Placement
    /// Creator: Benjamin
    /// </summary>
    public class BuildMenu : MonoBehaviour //Made by Ben
    {
        #region Variables

        //UI
        [SerializeField] private GameObject buildMenuLayout;
        private Camera mainCam;

        //Prefabs
        [SerializeField] private GameObject prefabBlueprintObject;
        [SerializeField] private GameObject prefabScaffold;
        [SerializeField] private Material bluePrintMaterial;
        
        private GameObject blueprintObject;
        private GameObject moduleToBuild;

        private Transform startModule;
        private Blueprint blueprint;
        
        private Vector3 mousePos;

        [SerializeField] private GameObject eventPanel;
        [SerializeField] private GameObject mileStonePanel;

        //Input
        [SerializeField] private PlayerInput playerInput;
        [SerializeField] private MeshRenderer[] children;
        private bool playerInputHasBeenInit;

        [SerializeField] private  GameObject buildMenu;

        #endregion

        #region Unity Events

        private void Start()
        {
            mainCam = Camera.main;
            startModule = GameObject.FindGameObjectWithTag("Station").transform;
            buildMenuLayout.SetActive(false);
        }

        private void Update()
        {
            if (!playerInputHasBeenInit && !eventPanel.activeSelf && !mileStonePanel.activeSelf)
            {
                InitPlayerInput();
            }

            if ((eventPanel.activeSelf || mileStonePanel.activeSelf) && playerInputHasBeenInit)
            {
                Terminate();
            }
        }

        #endregion

        #region Methods & Events

        /// <summary>
        /// unsubscribing event when no Input
        /// </summary>
        private void OnDisable()
        {
            if (playerInput == null) return;
            Terminate();
        }
        
        private void Terminate()
        {            
            playerInput.actions["LeftClick"].performed -= LeftMouseButtonPressed;
            playerInput.actions["OpenBuildMenu"].performed -= RightMouseButtonPressed;
            playerInputHasBeenInit = false;
            
        }

        /// <summary>
        /// Subscribing in Mouse Event
        /// </summary>
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
            //In Case of unused Blueprints, check them
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

        /// <summary>
        /// For Destroying unwanted Blueprints
        /// </summary>
        private void CheckIfBlueprintObjectExists()
        {
            if (blueprintObject != null)
            {
                Destroy(blueprintObject);
            }
        }

        /// <summary>
        /// for Placement in Grid by Leftclick
        /// </summary>
        /// <param name="context"></param>
        private void LeftMouseButtonPressed(InputAction.CallbackContext context)
        {
            //Quick return when there isn´t anything to build
            if (buildMenuLayout == null /*|| !buildMenuLayout.activeSelf*/)
                return;


            if (blueprintObject != null)
            {
                blueprint = blueprintObject.GetComponent<Blueprint>();

                if (blueprint.IsLockedIn)
                {
                    if (blueprint.gridTileHit.SetModuleOnUsed())
                    {
                        Vector3 spawnPos = blueprint.transform.position;
                     
                        //Instantiate Module on Pos
                        GameObject module = Instantiate(moduleToBuild, spawnPos,
                            quaternion.identity);
                        //For better visualizing in Editor
                        module.transform.parent = startModule;
                        //Deactivating Grid lock in
                        blueprint.gridTileHit.GetComponent<Collider>().isTrigger = false;
                        //Saving Module in Grid to remember 
                        blueprint.gridTileHit.Module = module;
                        module.SetActive(false);
                        
                        
                        //Activate Build Scaffold
                        GameObject scaffold = Instantiate(prefabScaffold, spawnPos,
                            Quaternion.identity);
                        scaffold.GetComponentInChildren<Scaffold>().Module = module;
                    }
                }

                buildMenuLayout.SetActive(false);
            }

            CheckIfBlueprintObjectExists();
            UnCheckAvailableGridTiles(true);
        }

        /// <summary>
        /// Starting Workflow when ui button pressed
        /// </summary>
        /// <param name="moduleToBuildGameObject"> the Object to build </param>
        public void BuildMenuButtonPressed(GameObject moduleToBuildGameObject)
        {
            buildMenu.SetActive(false);
            mousePos = Input.mousePosition;
            Vector3 spawnPos = mainCam.ScreenToWorldPoint(mousePos);
            CheckAvailableGridTiles();

            moduleToBuild = moduleToBuildGameObject;
            Transform model = moduleToBuildGameObject.transform.GetChild(0);

            CreateBlueprintModel(model,spawnPos);
        }

        /// <summary>
        /// Creating Blueprint Models with other Material
        /// </summary>
        /// <param name="model">model as child</param>
        /// <param name="spawnPos">Position to spawn</param>
        private void CreateBlueprintModel(Transform model, Vector3 spawnPos)
        {
            blueprintObject = Instantiate(prefabBlueprintObject, spawnPos, Quaternion.identity);
            Transform bpModel = Instantiate(model, spawnPos, quaternion.identity);
            bpModel.SetParent(blueprintObject.transform);
            SetBlueprintMaterial(bpModel);   
        }
        
        /// <summary>
        /// Change current materials to a monochrome blueprint color
        /// </summary>
        /// <param name="bpModel">Model to change</param>
        private void SetBlueprintMaterial(Transform bpModel)
        {
            children = bpModel.GetComponentsInChildren<MeshRenderer>();
            for (int i = 0; i < children.Length; i++)
            {
                MeshRenderer renderer = children[i];
                Material[] materials = new Material[renderer.materials.Length];
                for (int j = 0; j < renderer.sharedMaterials.Length; j++)
                {
                    materials[j] = bluePrintMaterial;
                }

                renderer.sharedMaterials = materials;
            }
        }

        /// <summary>
        /// local method to access singleton Gridsystem Checking
        /// </summary>
        private void CheckAvailableGridTiles()
        {
            Gridsystem.Gridsystem.Instance.CheckAvailableGridTilesAroundStation();
        }

        /// <summary>
        /// local method to access singleton Gridsystem Unchecking
        /// </summary>
        /// <param name="isBuilt"> nessesary for changes in runtime </param>
        private void UnCheckAvailableGridTiles(bool isBuilt)
        {
            Gridsystem.Gridsystem.Instance.UnCheckAvailableGridTilesAroundStation(isBuilt);
        }

        #endregion
    }
}