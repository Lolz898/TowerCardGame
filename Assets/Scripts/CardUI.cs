using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardUI : MonoBehaviour
{
    public Image cardArtworkImage;
    public TextMeshProUGUI cardNameText;
    public TextMeshProUGUI goldCostText;
    public TextMeshProUGUI manaCostText;
    public TextMeshProUGUI cardDescriptionText;
    public Image cardBackground;

    private CardScriptableObject cardData;
    public int playerHandIndex; // Index of the card in playerHand list
    private CardManager cardManager; // Reference to the CardManager instance
    public static List<CardUI> cardUIs = new List<CardUI>();

    private void Awake()
    {
        cardUIs.Add(this); // Add this instance to the list
    }

    public void Initialize(CardScriptableObject card, int handIndex, CardManager manager)
    {
        cardData = card;
        playerHandIndex = handIndex;
        cardManager = manager;
        UpdateCardData();
    }

    public void UpdateCardData()
    {
        cardArtworkImage.sprite = cardData.cardImage;
        cardNameText.text = cardData.cardName;

        // Update the gold cost text
        goldCostText.text = cardData.goldCost > 0 ? cardData.goldCost.ToString() : "";

        // Update the mana cost text
        manaCostText.text = cardData.manaCost > 0 ? cardData.manaCost.ToString() : "";

        cardDescriptionText.text = cardData.description;

        if (cardData.cardType == CardType.Tower)
        {
            cardBackground.color = Color.red;
        }
        else if (cardData.cardType == CardType.Spell)
        {
            cardBackground.color = Color.blue;
        }
    }

    public void UpdatePlayerHandIndex(int newIndex)
    {
        playerHandIndex = newIndex;
    }

    public void OnClick()
    {
        if (cardData.goldCost > GameManager.Instance.GetPlayerGold() & cardData.manaCost > GameManager.Instance.GetPlayerMana())
        {
            Debug.Log("Not enough gold and mana");
        }
        else if (cardData.goldCost > GameManager.Instance.GetPlayerGold())
        {
            Debug.Log("Not enough gold");
        }
        else if (cardData.manaCost > GameManager.Instance.GetPlayerMana())
        {
            Debug.Log("Not enough mana");
        }
        else if (cardData.cardType == CardType.Tower) // Only towers can be clicked to play
        {
            // Call the PlayCard method from the CardManager and pass the index of this card
            cardManager.StartCardPlay(playerHandIndex);
        }
    }

    public void DisableInteractions()   
    {
        GetComponent<Button>().interactable = false;
    }

    public void EnableInteractions()
    {
        GetComponent<Button>().interactable = true;
    }

    // Add methods here for handling card interactions, such as clicking or dragging
}
