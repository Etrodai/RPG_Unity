using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveTo : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private GameObject target;

    private void Awake()
    {
        agent = this.GetComponent<NavMeshAgent>();
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