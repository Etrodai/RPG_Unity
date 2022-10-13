using ResourceManagement;
using UnityEngine;

namespace MilestoneSystem
{
    [CreateAssetMenu(fileName = "New MileStone", menuName = "MileStones")]
    public class MileStonesScriptableObject : ScriptableObject
    {
        #region TODOS
        
        // Kamerasteuerung, ChangePrioListe                                         TODO
       
        #endregion

        #region Variables
        
        [SerializeField] [TextArea] private string[] mileStoneText;
        [SerializeField] private MileStoneEventNames[] requiredEvent;
        [SerializeField] private Resource[] requiredResources;
        [SerializeField] private MileStoneModules[] requiredModules;
        [SerializeField] [TextArea] private string[] mileStoneAchievedText;
    
        #endregion

        #region Properties
       
        public string[] MileStoneText => mileStoneText;
        public MileStoneEventNames[] RequiredEvent => requiredEvent;
        public Resource[] RequiredResources => requiredResources;
        public MileStoneModules[] RequiredModules => requiredModules;
        public string[] MileStoneAchievedText => mileStoneAchievedText;

        #endregion
    }
}