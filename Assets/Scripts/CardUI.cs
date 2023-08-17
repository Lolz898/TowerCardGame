using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class CardUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image cardArtworkImage;
    public TextMeshProUGUI cardNameText;
    public TextMeshProUGUI goldCostText;
    public TextMeshProUGUI manaCostText;
    public TextMeshProUGUI cardDescriptionText;
    public Image cardBackground;
    public int playerHandIndex; // Index of the card in playerHand list
    public static List<CardUI> cardUIs = new List<CardUI>();
    public bool shouldMoveDownOnExit = true;

    private CardScriptableObject cardData;
    private CardManager cardManager; // Reference to the CardManager instance
    private Vector3 originalPosition;
    private bool updatePosition = true;
    private Canvas cardCanvas;
    private int originalSortingOrder;
    private bool isHovering = false;

    private void Awake()
    {
        cardUIs.Add(this); // Add this instance to the list
        cardCanvas = GetComponent<Canvas>();

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
        else if (cardData.cardType == CardType.Tower || cardData.cardType == CardType.Spell) // Towers and spells can be played
        {
            // Call the PlayCard method from the CardManager and pass the index of this card
            cardManager.StartCardPlay(playerHandIndex);
            shouldMoveDownOnExit = !shouldMoveDownOnExit; // Toggle the value
            foreach (CardUI cardui in CardUI.cardUIs)
            {
                if (cardui != this)
                {
                    cardui.ResetCardPosition(false);
                }
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isHovering)
        {
            updatePosition = false;
            // Disable the HorizontalLayoutGroup
            HorizontalLayoutGroup layoutGroup = GetComponentInParent<HorizontalLayoutGroup>();
            if (layoutGroup != null)
            {
                layoutGroup.enabled = false;
            }

            if (cardCanvas != null)
            {
                cardCanvas.overrideSorting = true;
                originalSortingOrder = cardCanvas.sortingOrder;
                cardCanvas.sortingOrder = 10; // Set this to a value higher than other cards
            }

            Vector3 targetPosition = originalPosition + Vector3.up * 70f; // Adjust the hover distance
            transform.localPosition = targetPosition;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (shouldMoveDownOnExit)
        {
            bool shouldEnableLayoutGroup = true; // Default to enabling the layout group
            foreach (CardUI cardui in cardUIs)
            {
                shouldEnableLayoutGroup = false; // If any cardui has shouldMoveDownOnExit == false, disable the layout group
                break; // No need to check the rest, since one cardui is enough to disable the group
            }

            if (shouldEnableLayoutGroup)
            {
                // Enable the HorizontalLayoutGroup
                HorizontalLayoutGroup layoutGroup = GetComponentInParent<HorizontalLayoutGroup>();
                if (layoutGroup != null)
                {
                    layoutGroup.enabled = true;
                }
            }

            if (cardCanvas != null)
            {
                cardCanvas.sortingOrder = originalSortingOrder;
                cardCanvas.overrideSorting = false;
            }

            transform.localPosition = originalPosition;
            StartCoroutine(HoverTimer());

            updatePosition = true;
        }
    }

    private IEnumerator HoverTimer()
    {
        isHovering = true;
        yield return new WaitForSeconds(0.05f);
        isHovering = false;
    }

    public void ResetCardPosition(bool LayoutGroup)
    {
        shouldMoveDownOnExit = true;

        if (cardCanvas != null)
        {
            cardCanvas.sortingOrder = originalSortingOrder;
            cardCanvas.overrideSorting = false;
        }

        transform.localPosition = originalPosition;

        if (LayoutGroup)
        {
            HorizontalLayoutGroup layoutGroup = GetComponentInParent<HorizontalLayoutGroup>();
            if (layoutGroup != null)
            {
                layoutGroup.enabled = true;
            }
        }

        updatePosition = true;
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

    private void Update()
    {
        if (updatePosition)
        {
            originalPosition = transform.localPosition; // Store the original position
        }
    }
}
