using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GoldUI : MonoBehaviour
{
    private TMP_Text goldText;
    private GameManager gameManager;

    private void Awake()
    {
        goldText = GetComponent<TMP_Text>();
        gameManager = GameManager.Instance; // Make sure GameManager is a Singleton or accessible statically.
        UpdateGoldText(gameManager.GetPlayerGold()); // Call this to set the initial gold value on the UI
    }

    private void OnEnable()
    {
        gameManager.OnGoldUpdate += UpdateGoldText; // Subscribe to the gold update event
    }

    private void OnDisable()
    {
        gameManager.OnGoldUpdate -= UpdateGoldText; // Unsubscribe from the gold update event
    }

    private void UpdateGoldText(int newGold)
    {
        if (goldText != null)
        {
            goldText.text = newGold.ToString();
        }
    }
}
