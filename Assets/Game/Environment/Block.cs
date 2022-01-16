// Libraries.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(TilemapCollider2D))]
public class Block : MonoBehaviour {

    public bool isBeingUsed;

    void Update() {
        Color color = Color.white;
        if (isBeingUsed) {
            color.a *= 0.25f;
        }
        GetComponent<Tilemap>().color = color;
        isBeingUsed = false;
    }

}
