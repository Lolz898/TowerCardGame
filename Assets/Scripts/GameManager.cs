using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // Singleton instance of the GameManager

    private int playerGold = 50; // Current amount of gold the player has
    private int playerHealth = 100; // Current health of the player
    private int playerMana = 4; // Current mana of the player

    // Events for gold and health updates (you can use these to notify UI elements of changes)
    public delegate void OnGoldUpdateDelegate(int newGold);
    public delegate void OnHealthUpdateDelegate(int newHealth);
    public delegate void OnManaUpdateDelegate(int newMana);
    public event OnGoldUpdateDelegate OnGoldUpdate;
    public event OnHealthUpdateDelegate OnHealthUpdate;
    public event OnManaUpdateDelegate OnManaUpdate;

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

    // Method to set the player's gold
    public void SetPlayerGold(int gold)
    {
        playerHealth = Mathf.Max(0, gold);
        OnHealthUpdate?.Invoke(playerGold); // Notify listeners of gold update
    }

    // Method to set the player's health
    public void SetPlayerHealth(int health)
    {
        playerHealth = Mathf.Max(0, health);
        OnHealthUpdate?.Invoke(playerHealth); // Notify listeners of health update
    }

    // Method to modify the player's health (positive or negative)
    public void ModifyPlayerHealth(int amount)
    {
        playerHealth = Mathf.Max(0, playerHealth + amount);
        OnHealthUpdate?.Invoke(playerHealth); // Notify listeners of health update
    }

    // Method to get the current player health
    public int GetPlayerHealth()
    {
        return playerHealth;
    }

    // Method to set the player's mana
    public void SetPlayerMana(int mana)
    {
        playerMana = Mathf.Max(0, mana);
        OnManaUpdate?.Invoke(playerMana); // Notify listeners of mana update
    }

    public void AddMana(int amount)
    {
        playerMana += amount;
        OnManaUpdate?.Invoke(playerMana); // Notify listeners of mana update
    }

    // Method to remove mana from the player's balance
    public void RemoveMana(int amount)
    {
        playerMana = Mathf.Max(0, playerMana - amount);
        OnManaUpdate?.Invoke(playerMana); // Notify listeners of mana update
    }

    // Method to get the current player mana
    public int GetPlayerMana()
    {
        return playerMana;
    }

    // Optionally, you can add more game management functionality here.
}
