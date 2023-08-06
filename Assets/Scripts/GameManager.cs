using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // Singleton instance of the GameManager

    private int playerGold = 0; // Current amount of gold the player has

    // Events for gold updates (you can use these to notify UI elements of changes)
    public delegate void OnGoldUpdateDelegate(int newGold);
    public event OnGoldUpdateDelegate OnGoldUpdate;

    private void Awake()
    {
        // Ensure there's only one instance of the GameManager
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    // Method to add gold to the player's balance
    public void AddGold(int amount)
    {
        playerGold += amount;
        OnGoldUpdate?.Invoke(playerGold); // Notify listeners of gold update
    }

    // Method to remove gold from the player's balance
    public void RemoveGold(int amount)
    {
        playerGold = Mathf.Max(0, playerGold - amount);
        OnGoldUpdate?.Invoke(playerGold); // Notify listeners of gold update
    }

    // Method to get the current player gold balance
    public int GetPlayerGold()
    {
        return playerGold;
    }

    // Optionally, you can add more game management functionality here.
}
