using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Spell Card", menuName = "Card Game/Spell Card")]
public class SpellCardScriptableObject : CardScriptableObject
{
    // Add spell-specific attributes here
    public GameObject spellPrefab;
    public SpellTarget spellTarget;
    public bool hitAlly;
    public bool hitEnemy;
    public bool hitTower;
    public int damage;
    public int range;
}
