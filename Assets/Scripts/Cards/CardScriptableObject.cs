using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum CardType
{
    Tower,
    Spell
}

[CreateAssetMenu(fileName = "New Card", menuName = "Card Game/Card")]
public class CardScriptableObject : ScriptableObject
{
    public string cardName;
    public string description;
    public Sprite cardImage;
    public int manaCost;
    public int goldCost;
    public CardType cardType;

    // Additional fields specific to different card types (e.g., damage for tower cards, manaCost for spell cards, etc.)
}
