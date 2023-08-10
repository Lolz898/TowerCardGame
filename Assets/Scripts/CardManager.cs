using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public List<CardScriptableObject> drawPile = new List<CardScriptableObject>();
    public List<CardScriptableObject> playerHand = new List<CardScriptableObject>();
    public List<CardScriptableObject> discardPile = new List<CardScriptableObject>();

    public GameObject uiCardPrefab; // Assign the UI card prefab in the Inspector
    public Transform cardContainer; // Assign the card container in the Inspector

    private void Start()
    {
        DrawInitialCards();
    }

    private void DrawInitialCards()
    {
        for (int i = 0; i < 5; i++)
        {
            DrawCard();
        }
    }

    public void DrawCard()
    {
        if (drawPile.Count > 0)
        {
            int randomIndex = Random.Range(0, drawPile.Count);
            CardScriptableObject drawnCard = drawPile[randomIndex];
            drawPile.RemoveAt(randomIndex);
            playerHand.Add(drawnCard);

            // Instantiate the UI card prefab and set its parent and position
            GameObject uiCardObject = Instantiate(uiCardPrefab, cardContainer);
            CardUI uiCard = uiCardObject.GetComponent<CardUI>();
            uiCard.Initialize(drawnCard);
        }
    }

    public void PlayCard(int handIndex)
    {
        if (handIndex >= 0 && handIndex < playerHand.Count)
        {
            CardScriptableObject cardToPlay = playerHand[handIndex];
            playerHand.RemoveAt(handIndex);

            if (cardToPlay.cardType == CardType.Spell) // Check if the card is a spell card
            {
                discardPile.Add(cardToPlay); // Add the played spell card to the discard pile
            }
        }
    }
}
