using UnityEngine;

namespace Prototype.Eventsystem
{
    [CreateAssetMenu(fileName = "new Event", menuName = "Events")]
    public class EventScriptableObject : ScriptableObject
    {
        [SerializeField] [TextArea] private string eventText;
        [SerializeField] private DecisionsAndConsequences[] decisions;
    }
}
