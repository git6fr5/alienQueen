// Libraries.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Biomass {

    // Properties.
    public float value;

    public static void Drop(Vector3 position) {
        // Create the object.
        GameObject gameObject = new GameObject("Biomass", typeof(SpriteRenderer));
        gameObject.transform.position = position;
        // Set up the visuals.
        SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = GameRules.BiomassSprite;
        // Set up the values.
        gameObject.SetActive(true);
    }

}
