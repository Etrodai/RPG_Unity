using ResourceManagement;
using UnityEngine;

namespace MilestoneSystem
{
    [CreateAssetMenu(fileName = "New MileStone", menuName = "MileStones")]
    public class MileStonesScriptableObject : ScriptableObject
    {
        #region Variables
        
        [SerializeField] [TextArea] private string[] mileStoneText;
        [SerializeField] private MileStoneEventName[] requiredEvent;
        [SerializeField] private Resource[] requiredResources;
        [SerializeField] private MileStoneModule[] requiredModules;
        [SerializeField] [TextArea] private string[] mileStoneAchievedText;
    
        #endregion

        #region Properties
       
        public string[] MileStoneText => mileStoneText;
        public MileStoneEventName[] RequiredEvent => requiredEvent;
        public Resource[] RequiredResources => requiredResources;
        public MileStoneModule[] RequiredModules => requiredModules;
        public string[] MileStoneAchievedText => mileStoneAchievedText;

        #endregion
    }
}