using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Spell Card", menuName = "Card Game/Spell Card")]
public class SpellCardScriptableObject : CardScriptableObject
{
    // Add spell-specific attributes here
    public string spellTarget;
    public string spellEffect;
}
