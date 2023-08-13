using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Tower Card", menuName = "Card Game/Tower Card")]
public class TowerCardScriptableObject : CardScriptableObject
{
    // Add tower-specific attributes here
    public GameObject towerPrefab;
    public float range;
    public int damage;
    public float fireRate;
}




