using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static Unit;

[RequireComponent(typeof(NavMeshAgent))]
public class UnitMovement : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private Unit unit;
    private Transform target; // The current movement target
    private bool isEnemyUnit; // Store if the unit is an enemy during initialization
    private int currentWaypointIndex = 0; // Keep track of the current waypoint
    private List<Transform> waypoints = new List<Transform>(); // Store waypoints

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        unit = GetComponent<Unit>();
        isEnemyUnit = unit != null && unit.isEnemy;

        // Find the parent GameObject "Waypoints"
        Transform waypointsParent = GameObject.Find("Waypoints").transform;

        // Populate the waypoints list with the children of the waypoints parent
        foreach (Transform waypoint in waypointsParent)
        {
            waypoints.Add(waypoint);
        }

        if (waypoints.Count > 0)
        {
            SetNextWaypoint();
        }
    }
    public void SetNextWaypoint()
    {
        if (currentWaypointIndex < waypoints.Count)
        {
            target = waypoints[currentWaypointIndex];
            navMeshAgent.SetDestination(target.position);
        }
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

    private void Update()
    {
        // Ensure only enemy units follow waypoints
        if (isEnemyUnit && unit.currentStatus != UnitStatus.AttackingTarget)
        {
            if (waypoints.Count > 0 && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance + 0.7)
            {
                currentWaypointIndex++;
                if (currentWaypointIndex < waypoints.Count)
                {
                    SetNextWaypoint();
                }
                else
                {
                    // Implement behavior when the end of the path is reached
                    Debug.Log("End of path reached");
                    Destroy(gameObject); // For example, destroy the unit
                }
            }
        }
    }
}
