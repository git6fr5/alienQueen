using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Nest {

    // Properties.
    [SerializeField] public List<Alien> aliens = new List<Alien>();
    [SerializeField, ReadOnly] public float hatchRadius;
    [SerializeField, ReadOnly] public Alien selectedAlien;

    public Nest(float hatchRadius) {
        this.hatchRadius = hatchRadius;
        aliens = new List<Alien>();
    }

    public void AddToNest(Alien alien) {
        aliens.Add(alien);
    }

    public void Update(float deltaTime, bool clicked) {
        for (int i = 0; i < aliens.Count; i++) {
            aliens[i].OnUpdate(deltaTime, clicked);
        }
        // Filter through all the selections.
        FilterSelections();
    }

    private void FilterSelections() {
        for (int i = 0; i < aliens.Count; i++) {
            if (aliens[i].selector.isSelected && selectedAlien != aliens[i]) {
                if (selectedAlien != null) {
                    selectedAlien.selector.isSelected = false;
                }
                selectedAlien = aliens[i];
            }
        }
    }
}

