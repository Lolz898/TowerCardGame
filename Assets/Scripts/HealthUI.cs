using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HealthUI : MonoBehaviour
{
    private TMP_Text healthText;
    private GameManager gameManager;

    private void Awake()
    {
        gameManager = GameManager.Instance; // Make sure GameManager is a Singleton or accessible statically.
        healthText = GetComponent<TMP_Text>();
        UpdateHealthText(gameManager.GetPlayerHealth()); // Call this to set the initial health value on the UI
    }

    private void OnEnable()
    {
        gameManager.OnHealthUpdate += UpdateHealthText; // Subscribe to the health update event
    }

    private void OnDisable()
    {
        gameManager.OnHealthUpdate -= UpdateHealthText; // Unsubscribe from the health update event
    }

    private void UpdateHealthText(int newHealth)
    {
        if (healthText != null)
        {
            healthText.text = newHealth.ToString();
        }
    }
}
