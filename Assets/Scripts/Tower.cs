using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    // Tower attributes
    public float range = 5.0f;
    public int damage = 10;
    public float fireRate = 1.0f;
    public int goldCost = 50;

    // Reference to the enemy currently targeted
    private Unit currentTarget;

    // Timer to track the time since the last shot
    private float shotTimer = 0.0f;

    // Method to set the target for the tower
    public void SetTarget(Unit unit)
    {
        currentTarget = unit;
    }

    // Method to clear the tower's current target
    public void ClearTarget()
    {
        currentTarget = null;
    }

    // Method to check if the tower can shoot
    private bool CanShoot()
    {
        return currentTarget != null && Vector3.Distance(transform.position, currentTarget.transform.position) <= range;
    }

    // Method to handle tower shooting
    private void Shoot()
    {
        // Implement tower shooting logic here
        // For example, deal damage to the target
        if (currentTarget != null)
        {
            currentTarget.TakeDamage(damage);
        }
    }

    // Update is called once per frame
    private void Update()
    {
        // Check if the tower can shoot and if enough time has passed since the last shot
        if (CanShoot() && shotTimer >= 1.0f / fireRate)
        {
            // Perform the tower's shooting action
            Shoot();
            shotTimer = 0.0f; // Reset the shot timer
        }

        // Increment the shot timer
        shotTimer += Time.deltaTime;
    }

    // Optionally, you can add more methods or functionality specific to the tower here.
}
