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

    private CardScriptableObject cardData;

    public void Initialize(CardScriptableObject card)
    {
        cardData = card;
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
    }

    // Add methods here for handling card interactions, such as clicking or dragging
}
