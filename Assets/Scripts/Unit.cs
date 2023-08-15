using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Unit : MonoBehaviour
{
    public enum UnitStatus
    {
        Idle,
        MovingToNextWaypoint,
        MovingToTarget,
        AttackingTarget,
        Dead
    }

    public UnitStatus currentStatus = UnitStatus.Idle; // Current status of the unit
    public int maxHP = 100;          // Maximum health points
    public int currentHP;           // Current health points
    public float movementSpeed = 5f; // Movement speed in units per second
    public int goldReward = 10;     // Amount of gold rewarded when the unit is defeated
    public float sightRange = 10f;  // Sight range in units
    public float attackRange = 2f;  // Attack range in units
    public bool isEnemy = true;     // Boolean to indicate if the unit is an enemy (true) or an ally (false)

    public float attackRate = 1f;   // Attack rate in attacks per second
    public int attackDamage = 10;   // Amount of damage dealt per attack

    private Transform target;       // Current movement target
    private List<Unit> targetsInRange = new List<Unit>(); // List of valid targets within sight range
    private float timeSinceLastAttack; // Time elapsed since the last attack

    private UnitMovement unitMovement; // Reference to the UnitMovement script
    private NavMeshAgent navMeshAgent;

    private void Awake()
    {
        unitMovement = GetComponent<UnitMovement>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        currentHP = maxHP;
        navMeshAgent.speed = movementSpeed;
        timeSinceLastAttack = attackRate; // Initialize the time since last attack to the attack rate, so the unit can attack immediately if there is a target
    }

    // Method to set the movement target for the unit
    private void SetMovementTarget(Transform newTarget)
    {
        target = newTarget;
        unitMovement.SetTarget(target);
    }

    // Method to handle taking damage
    public virtual void TakeDamage(int damageAmount)
    {
        currentHP -= damageAmount;
        Debug.Log(gameObject.name + "took " + damageAmount + " damage! Current HP: " + currentHP);
        if (currentHP <= 0)
        {
            Die();
        }
    }

    // Method to handle unit death
    public virtual void Die()
    {
        currentStatus = UnitStatus.Dead;
        Collider collider = GetComponent< Collider>();
        collider.enabled = false;
        // Implement death logic here, such as awarding gold to the player
        // and destroying the GameObject.
        GameManager.Instance.AddGold(goldReward);
        Destroy(gameObject);
    }

    // Method to find valid targets within the sight range based on the faction
    private void FindTargetsInRange()
    {
        targetsInRange.Clear();

        // Get all colliders within the sight range
        Collider[] colliders = Physics.OverlapSphere(transform.position, sightRange);

        foreach (Collider collider in colliders)
        {
            // Check if the collider belongs to another unit
            Unit unit = collider.GetComponent<Unit>();
            if (unit != null && unit != this)
            {
                // Check if the unit is an enemy or an ally based on their isEnemy value
                // and add them to the list of targets in range
                if ((isEnemy && !unit.isEnemy) || (!isEnemy && unit.isEnemy))
                {
                    targetsInRange.Add(unit);
                }
            }
        }
    }

    // Method to find the closest target within sight range
    private Unit FindClosestTarget()
    {
        Unit closestTarget = null;
        float closestDistance = float.MaxValue;

        foreach (Unit target in targetsInRange)
        {
            float distance = Vector3.Distance(transform.position, target.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestTarget = target;
            }
        }

        return closestTarget;
    }

    // Method to attack the current target
    private void AttackTarget()
    {
        if (timeSinceLastAttack >= 1f / attackRate)
        {
            // Implement attack logic here
            // For example, deal damage to the target
            Unit target = FindClosestTarget();
            if (target != null)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);
                if (distanceToTarget <= attackRange) // Check if the target is within attack range
                {
                    target.TakeDamage(attackDamage);
                    timeSinceLastAttack = 0f; // Reset the time since last attack
                }

            }
        }
    }

    // Method to handle unit movement
    private void HandleMovement()
    {
        // If the unit has a target within sight range, check if it is within attack range
        Unit closestTarget = FindClosestTarget();
        if (closestTarget != null)
        {
            float distanceToTarget = Vector3.Distance(transform.position, closestTarget.transform.position);
            Debug.Log("Closest= " + closestTarget.name + " Distance= " + distanceToTarget + " Sight= " + sightRange + " Attack= " + attackRange);
            if (distanceToTarget <= sightRange + 0.15f && distanceToTarget > attackRange)
            {
                SetMovementTarget(closestTarget.transform); // Move towards the target
                currentStatus = UnitStatus.MovingToTarget;
            }
            else
            {
                unitMovement.StopMovement(); // Stop movement if within attack range
                currentStatus = UnitStatus.AttackingTarget;
            }
        }
        else
        {
            if (isEnemy)
            {
                unitMovement.SetNextWaypoint(); // Resume waypoint following for enemy units
                currentStatus = UnitStatus.MovingToNextWaypoint;
            }
            else
            {
                unitMovement.StopMovement(); // Stop movement if there is no target
                currentStatus = UnitStatus.Idle;
            }
        }
    }

    // Update is called once per frame
    private void Update()
    {
        // Update the list of targets within sight range
        FindTargetsInRange();

        // Handle unit behavior and targeting
        HandleMovement();
        AttackTarget();

        // Increment the time since the last attack
        timeSinceLastAttack += Time.deltaTime;
    }

    // Optionally, you can add more methods or functionality common to all units here.
}
