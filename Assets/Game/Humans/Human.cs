using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Params = GameRules.Params;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class Human : MonoBehaviour {

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

    private float thinkTicks;
    public float thinkInterval;

    // Initializes the alien.
    void Start() {
        Init();
    }

    public virtual void Init() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        target.position = transform.position;
        gameObject.SetActive(true);
    }

    // Runs once per frame.
    void Update() {
        CheckSelect();
        GetTarget();
        Move();
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

    void GetTarget() {
        thinkTicks += Time.deltaTime;
        if (thinkTicks > thinkInterval) {
            target.position = Random.insideUnitCircle * 5f;
            thinkTicks -= thinkInterval;
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
