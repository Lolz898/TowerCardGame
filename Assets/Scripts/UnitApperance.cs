using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitApperance : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        // Get a reference to the sprite renderer component
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        // Calculate the difference between this object's rotation and 0
        float rotationDifference = transform.eulerAngles.y;

        // Set the sprite's local rotation to the calculated difference
        spriteRenderer.transform.localRotation = Quaternion.Euler(90f, -rotationDifference, 0f);
    }
}
