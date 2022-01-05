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
    public Slider storageSlider;

    // Runs once before the first frame.
    void Start() {
        // Caching.
        alien = transform.parent.GetComponent<Alien>();
    }

    // Runs once every frame.
    void Update() {
        DisplayBiomass();
    }

    void DisplayBiomass() {

        if (!alien.isSelected) {
            HideDisplay(storageSlider.gameObject);
            return;
        }
        ShowDisplay(storageSlider.gameObject);

        float storage = alien.biomass / alien.maxBiomass;
        storageSlider.value = storage;

    }

    void HideDisplay(GameObject gameObject) {
        gameObject.SetActive(false);
    }

    void ShowDisplay(GameObject gameObject) {
        gameObject.SetActive(true);
    }

}
