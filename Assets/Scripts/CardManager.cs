using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public static CardManager Instance;

    public List<CardScriptableObject> drawPile = new List<CardScriptableObject>();
    public List<CardScriptableObject> playerHand = new List<CardScriptableObject>();
    public List<CardScriptableObject> discardPile = new List<CardScriptableObject>();

    public GameObject uiCardPrefab; // Assign the UI card prefab in the Inspector
    public Transform cardContainer; // Assign the card container in the Inspector

    private bool isPlacingTower = false;
    private GameObject ghostTower;
    private GameObject towerPrefabToPlace; // Reference to the tower prefab to be placed
    private CardScriptableObject cardToPlay; // Reference that holds the card being played
    private CardUI activeCardUI; // Reference used to hold the card ui linked to the actual card stored in data

    private void Awake()
    {
        // Ensure there's only one instance of the CardManager
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        DisplayDrawPile("Draw Pile at Start:");
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

            if (playerHand.Count < 10)
            {
                playerHand.Add(drawnCard);

                DisplayDrawPile("Draw Pile:");

                // Instantiate the UI card prefab and set its parent and position
                GameObject uiCardObject = Instantiate(uiCardPrefab, cardContainer);
                CardUI uiCard = uiCardObject.GetComponent<CardUI>();
                uiCard.Initialize(drawnCard, playerHand.Count - 1, this); // Pass hand index and CardManager instance
            }
            else
            {
                discardPile.Add(drawnCard);

                Debug.Log(drawnCard.cardName + " drawn to discard pile");
            }

        }
    }

    public void StartCardPlay(int handIndex)
    {
        if (handIndex >= 0 && handIndex < playerHand.Count)
        {
            cardToPlay = playerHand[handIndex];

            Debug.Log("Card played: " + cardToPlay.cardName);

            activeCardUI = CardUI.cardUIs[handIndex];

            if (cardToPlay.cardType == CardType.Spell) // Check if the card is a spell card
            {
                discardPile.Add(cardToPlay); // Add the played spell card to the discard pile
            }
            else if (cardToPlay.cardType == CardType.Tower) // Check if the card is a tower card
            {
                // Handle tower card placement logic here
                // Instantiate the tower prefab at the chosen location
                BeginPlacement((TowerCardScriptableObject)cardToPlay);
            }
        }
    }

    private void DisplayDrawPile(string message)
    {
        for (int i = 0; i < drawPile.Count; i++)
        {
            Debug.Log(message + " Index: " + i + " - Card Name: " + drawPile[i].cardName);
        }
    }

    private void BeginPlacement(TowerCardScriptableObject towerCard)
    {
        isPlacingTower = true;

        towerPrefabToPlace = towerCard.towerPrefab; // Store the tower prefab for placement
        // Instantiate the ghost tower sprite at the mouse cursor's position
        ghostTower = Instantiate(towerCard.towerPrefab, Input.mousePosition, Quaternion.identity);

        // Disable the ghost tower's collider so it doesn't interfere with placement
        ghostTower.GetComponent<Collider>().enabled = false;

        foreach (CardUI cardUI in CardUI.cardUIs)
        {
            cardUI.DisableInteractions();
        }
    }

    private void CheckTowerPlacement()
    {
        // Update the position of the ghost tower sprite to follow the cursor
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.y = 10f;
        ghostTower.transform.position = mousePosition;

        // Check if the player has clicked to place the tower
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                Node node = hit.collider.GetComponent<Node>();
                // Check if the hit collider is a node and if it's not occupied
                if (node != null && node.IsAvailableForPlacement())
                {
                    ConstructTower(node);
                }
            }
        }
    }

    private void ConstructTower(Node node)
    {
        // Place the tower on the node
        node.PlaceTower(towerPrefabToPlace);

        CardPlayCleanup();

        // Clean up ghost tower and reset placement mode
        Destroy(ghostTower);
        isPlacingTower = false;

        // Re-enable card interactions
        foreach (CardUI cardUI in CardUI.cardUIs)
        {
            cardUI.EnableInteractions();
        }
    }

    private void CardPlayCleanup()
    {
        // Deduct mana and gold based on cardToPlay's manaCost and goldCost
        if (cardToPlay.manaCost > 0)
        {
            GameManager.Instance.RemoveMana(cardToPlay.manaCost);
        }

        if (cardToPlay.goldCost > 0)
        {
            GameManager.Instance.RemoveGold(cardToPlay.goldCost);
        }

        playerHand.RemoveAt(activeCardUI.playerHandIndex);
        // Destroy the CardUI associated with the played card
        if (activeCardUI != null)
        {
            Destroy(activeCardUI.gameObject);
            int destroyedHandIndex = activeCardUI.playerHandIndex;
            CardUI.cardUIs.Remove(activeCardUI);
            activeCardUI = null;

            for (int i = destroyedHandIndex; i < playerHand.Count; i++)
            {
                CardUI.cardUIs[i].UpdatePlayerHandIndex(i);
            }
        }
    }

    private void Update()
    {
        if (isPlacingTower)
        {
            CheckTowerPlacement();
        }
    }
}
