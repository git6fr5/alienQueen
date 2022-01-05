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
    public Transform target;
    public Params speed;
    public float power;
    public float damping;
    private Vector3 velocity;

    public bool isSelected;
    public bool isMouseOver;

    // Initializes the alien.
    public virtual void Init(Queen queen) {

        transform.position = queen.transform.position + (Vector3)(Random.insideUnitCircle.normalized) * queen.hatchRadius;
        target.position = transform.position;
        spriteRenderer = GetComponent<SpriteRenderer>();

        this.queen = queen;
        gameObject.SetActive(true);
    }

    // Runs once per frame.
    void Update() {
        CheckSelect();
        GetTarget();
        Move();
        // Seperation();
    }

    void Move() {

        Vector3 displacement = (target.position - transform.position);
        if (displacement.sqrMagnitude < 0.05f * 0.05f) {
            velocity = Vector3.zero;
            return;
        }

        float deltaTime = Time.deltaTime;

        Vector3 direction = displacement.normalized;
        Vector3 acceleration = direction * power;
        velocity += acceleration * deltaTime;
        velocity = speed.Clamp(velocity.magnitude) * velocity.normalized;
        velocity *= damping;

        transform.position += velocity * deltaTime;

    }

    float seperationThreshold = 0.5f;
    void Seperation() {

        Vector3 seperationForce = Vector3.zero;

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, seperationThreshold);
        for (int i = 0; i < hits.Length; i++) {
            Alien alien = hits[i].GetComponent<Alien>();
            if (alien != null && alien != this) {
                Vector3 displacement = (transform.position - alien.transform.position);
                seperationForce -= displacement.normalized * 20f * (seperationThreshold - displacement.magnitude);
            }
        }

        transform.position += seperationForce * Time.deltaTime;

    }

    void GetTarget() {
        if (isSelected && Input.GetMouseButtonDown(1)) {
            target.parent = null;
            target.position = GameRules.MousePosition - Vector3.forward * GameRules.MousePosition.z;
        }
    }

    void CheckSelect() {

        if (Input.GetMouseButtonDown(0)) {
            if (Input.GetKey(KeyCode.LeftShift)) {
                if (isMouseOver) {
                    isSelected = true;
                }
            }
            else {
                isSelected = isMouseOver;
            }
        }

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

    void OnDrawGizmos() {
        Gizmos.DrawLine(transform.position, target.position);
    }

}
