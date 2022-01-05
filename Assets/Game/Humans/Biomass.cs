using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class Biomass : MonoBehaviour {

    SpriteRenderer spriteRenderer;

    public float value;

    public void Init(Vector3 position, float value) {
        transform.position = position;
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = GameRules.BiomassSprite;
        this.value = value;
        gameObject.SetActive(true);
    }

}
