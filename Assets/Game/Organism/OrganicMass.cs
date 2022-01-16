// Libraries.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrganicMass : MonoBehaviour {

    #region Properties
    [SerializeField, ReadOnly] public float value;
    [SerializeField, ReadOnly] public bool collectible;
    #endregion

    #region Methods
    public void Init(float value, bool collectible) {
        this.value = value;
        this.collectible = collectible;
    }
    #endregion

    #region Static Methods
    public static void Drop(Vector3 position, float value) {
        // Create the object.
        GameObject gameObject = new GameObject("Biomass", typeof(SpriteRenderer), typeof(OrganicMass));
        gameObject.transform.position = position;
        // Set up the visuals.
        SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = GameRules.BiomassSprite;
        // Set up the mass.
        OrganicMass organicMass = gameObject.GetComponent<OrganicMass>();
        organicMass.Init(value, true);
        // Set up the values.
        gameObject.SetActive(true);
    }
    #endregion

}
