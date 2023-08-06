using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    private bool isOccupied = false; // Whether the node is occupied by a tower
    private GameObject placedTower; // Reference to the tower placed on this node (if any)
    private GameObject towerPrefab; // The prefab of the tower to be placed on this node

    // Method to check if the node is available for tower placement
    public bool IsAvailableForPlacement()
    {
        return !isOccupied;
    }

    // Method to place a tower on this node
    public void PlaceTower(GameObject towerPrefabToPlace)
    {
        // Check if the node is available for placement
        if (!IsAvailableForPlacement())
        {
            Debug.LogWarning("Node is already occupied by a tower.");
            return;
        }

        // Instantiate the tower prefab as a child of the node
        placedTower = Instantiate(towerPrefabToPlace, transform.position, Quaternion.identity);
        placedTower.transform.SetParent(transform); // Set the node as the parent of the tower
        towerPrefab = towerPrefabToPlace; // Store the tower prefab for reference
        isOccupied = true; // Mark the node as occupied by a tower
    }

    // Method to remove the tower from this node (optional)
    public void RemoveTower()
    {
        // Check if there is a tower placed on the node
        if (isOccupied && placedTower != null)
        {
            // Destroy the tower GameObject
            Destroy(placedTower);
            placedTower = null;
            towerPrefab = null; // Reset the tower prefab reference
            isOccupied = false; // Mark the node as available for tower placement
        }
    }

    // Optionally, you can add methods for highlighting the node when it's available for tower placement
    // and removing the highlight when it's not available.

    // Optionally, you can add any other methods or functionality specific to the node here.

    // Optionally, you can use events or delegate patterns to notify other parts of the game about tower placement or removal.
}
