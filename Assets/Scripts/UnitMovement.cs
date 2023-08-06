using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class UnitMovement : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private Transform target; // The current movement target

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Method to set the movement target for the unit
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;

        // Set the destination for the NavMeshAgent
        if (navMeshAgent != null && target != null)
        {
            navMeshAgent.SetDestination(target.position);
        }
    }

    // Method to stop the unit's movement
    public void StopMovement()
    {
        target = null;
        if (navMeshAgent != null)
        {
            navMeshAgent.ResetPath();
        }
    }
}
