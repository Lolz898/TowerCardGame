using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerAppearance : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        // Get a reference to the sprite renderer component
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        // Set the sprite's local rotation to the calculated difference
        spriteRenderer.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
    }
}
