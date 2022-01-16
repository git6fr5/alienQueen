// Libraries.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(OrganicBody))]
public class Organism : MonoBehaviour {

    #region Components
    [HideInInspector] public OrganicBody organicBody;
    [HideInInspector] public SpriteRenderer spriteRenderer;
    #endregion

    #region Parameters
    [SerializeField] private int maxHealth = 0; // The maximum health of this organism.
    #endregion

    #region Properties
    [SerializeField, ReadOnly] private int health = 0; // The current health of this organism.
    #endregion

    #region Unity
    // Runs once before the first frame.
    private void Start() {
        Init();
    }

    // Runs once every frame.
    private void Update() {
        // Cache the time differential.
        float deltaTime = Time.deltaTime;

    }
    #endregion

    #region Commands
    // Runs the action of this organism.
    public virtual void Action() {
        // Determined by the type of organism.
        Debug.Log("Performing an action");
    }
    #endregion

    #region Methods
    // Run to initialize the script.
    protected virtual void Init() {
        // Cache the components.
        organicBody = GetComponent<OrganicBody>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Set the properties.
        health = maxHealth;

        // Activate the object.
        gameObject.SetActive(true);
    }
    #endregion
}
