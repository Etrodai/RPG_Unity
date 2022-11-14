using Eventsystem;
using ResourceManagement.Manager;
using UnityEngine;

namespace Manager
{
    [RequireComponent(typeof(EnergyManager), typeof(FoodManager), typeof(WaterManager))]
    [RequireComponent(typeof(MaterialManager), typeof(CitizenManager), typeof(GameManager))]
    [RequireComponent(typeof(EventManagerScriptable))]
    public class MainManagerSingleton : MonoBehaviour
    {
        public static MainManagerSingleton Instance { get; private set; }

        //All other managers
        public CitizenManager CitizenManager { get; private set; }
        public EnergyManager EnergyManager { get; private set; }
        public FoodManager FoodManager { get; private set; }
        public WaterManager WaterManager { get; private set; }
        public MaterialManager MaterialManager { get; private set; }
        public GameManager GameManager { get; private set; }
        public EventManagerScriptable EventManager { get; private set; }

        private void Awake()
        {
            if(Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }

            CitizenManager = gameObject.GetComponent<CitizenManager>();
            EnergyManager = gameObject.GetComponent<EnergyManager>();
            FoodManager = gameObject.GetComponent<FoodManager>();
            WaterManager = gameObject.GetComponent<WaterManager>();
            MaterialManager = gameObject.GetComponent<MaterialManager>();
            GameManager = gameObject.GetComponent<GameManager>();
            EventManager = gameObject.GetComponent<EventManagerScriptable>();
        }
    }
}
