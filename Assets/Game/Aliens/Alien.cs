using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Params = GameRules.Params;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class Alien : MonoBehaviour {

    // Components.
    Queen queen;
    SpriteRenderer spriteRenderer;

    // Properties.
    [HideInInspector] public float horizontal;
    [HideInInspector] public float vertical;

    public Params speed;
    public float power;
    public float damping;
    private Vector3 velocity;

    public bool attack;
    public float biomass;
    public float maxBiomass;

    public bool isSelected;
    public bool isMouseOver;

    // Initializes the alien.
    public virtual void Init(Queen queen) {

        transform.position = queen.transform.position + (Vector3)(Random.insideUnitCircle.normalized) * queen.hatchRadius;
        spriteRenderer = GetComponent<SpriteRenderer>();

        this.queen = queen;
        gameObject.SetActive(true);
    }

    // Runs once per frame.
    void Update() {
        CheckSelect();
        Move();
        if (attack) {
            Attack();
            attack = false;
        }
    }

    void Move() {

        float deltaTime = Time.deltaTime;

        Vector3 direction = new Vector3(horizontal, vertical, 0f).normalized;
        Vector3 acceleration = direction * power;
        velocity += acceleration * deltaTime;
        velocity = speed.Clamp(velocity.magnitude) * velocity.normalized;

        if (acceleration == Vector3.zero) {
            velocity *= damping;
        }

        transform.position += velocity * deltaTime;

        horizontal = 0f;
        vertical = 0f;

    }

    void Attack() {

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 1f);
        for (int i = 0; i < hits.Length; i++) {
            Human human = hits[i].GetComponent<Human>();
            if (human != null) {
                human.Hurt(1);
                return;
            }

            Biomass biomass = hits[i].GetComponent<Biomass>();
            if (biomass != null) {
                Eat(biomass);
                return;
            }

            QueenUI queenUI = hits[i].GetComponent<QueenUI>();
            if (queenUI != null) {
                queenUI.queen.StoreBiomass(this);
                return;
            }
        }
    }

    void Eat(Biomass biomass) {
        if (this.biomass + biomass.value < maxBiomass) {
            this.biomass += biomass.value;
            Destroy(biomass.gameObject);
        }
    }

    void CheckSelect() {

        if (isSelected) {
            spriteRenderer.material.SetFloat("_OutlineWidth", GameRules.OutlineWidth);
        }
        else {
            spriteRenderer.material.SetFloat("_OutlineWidth", 0f);
        }

    }

    void OnMouseOver() {
        isMouseOver = true;
    }

    void OnMouseExit() {
        isMouseOver = false;
    }

}
