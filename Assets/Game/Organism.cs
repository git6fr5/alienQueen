using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Organism : MonoBehaviour {

    [SerializeField] public float maxHealth;
    [SerializeField, ReadOnly] public float health;

    void Start() {
        health = maxHealth;
    }

}
