// Libraries.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlienUI : MonoBehaviour {

    // Components.
    [HideInInspector] public Alien alien;
    Canvas canvas;

    // Progress.
    [Space(5), Header("Sliders")]
    public Slider healthSlider;

    // Properties.
    [SerializeField, ReadOnly] private bool isEnabled;

    // Run to initialize.
    public void Init(Alien alien) {
        // Caching.
        canvas = GetComponent<Canvas>();
        this.alien = alien;
        gameObject.SetActive(true);
    }

    public void OnUpdate() {
        isEnabled = alien.selector.isSelected;
        Display();
    }

    void Display() {
        DisplayHealth();
    }

    void DisplayHealth() {

        float health = alien.health / alien.maxHealth;
        healthSlider.value = health;

    }

    void HideDisplay(GameObject gameObject) {
        gameObject.SetActive(false);
    }

    void ShowDisplay(GameObject gameObject) {
        gameObject.SetActive(true);
    }

}
