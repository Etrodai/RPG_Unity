using ResourceManagement;
using UnityEngine;

namespace Buildings
{
    [CreateAssetMenu(fileName = "New Building", menuName = "Buildings")]
    public class BuildingResourcesScriptableObject : ScriptableObject //Made by Robin
    {
        #region Variables
        
        [SerializeField] private Resource[] costs;
        [SerializeField] private Resource[] consumption;
        [SerializeField] private Resource[] production;
        [SerializeField] private Resource[] saveSpace;
      
        #endregion

        #region Properties
        
        public Resource[] Costs => costs;
        public Resource[] Consumption => consumption;
        public Resource[] Production => production;
        public Resource[] SaveSpace => saveSpace;
        
        #endregion
    }
}