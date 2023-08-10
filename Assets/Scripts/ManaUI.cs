using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ManaUI : MonoBehaviour
{
    private TMP_Text manaText;
    private GameManager gameManager;

    private void Awake()
    {
        manaText = GetComponent<TMP_Text>();
        gameManager = GameManager.Instance; // Make sure GameManager is a Singleton or accessible statically.
        UpdateManaText(gameManager.GetPlayerMana()); // Call this to set the initial gold value on the UI
    }

    private void OnEnable()
    {
        gameManager.OnManaUpdate += UpdateManaText; // Subscribe to the gold update event
    }

    private void OnDisable()
    {
        gameManager.OnManaUpdate -= UpdateManaText; // Unsubscribe from the gold update event
    }

    private void UpdateManaText(int newMana)
    {
        if (manaText != null)
        {
            manaText.text = newMana.ToString();
        }
    }
}
