using UnityEngine;
using UnityEngine.AI;

namespace Testing
{
    public class MoveTo : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private GameObject target;

        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
        }

        private void Update()
        {
            Vector3 targetPos = target.transform.position;

            agent.SetDestination(targetPos);


            if (agent.remainingDistance <= 15f)
            {
                agent.ResetPath();
            }
        }
    }
}