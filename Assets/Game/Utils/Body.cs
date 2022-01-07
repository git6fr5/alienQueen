using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Body {

    // Properties.
    [Space(5), Header("Internal Controls")]
    [SerializeField, ReadOnly] private bool isEnabled;
    [SerializeField, ReadOnly] private bool useGravity;
    [SerializeField, ReadOnly] private Vector3 target;
    [SerializeField, ReadOnly] private Vector3 velocity;

    [Space(5), Header("Settings")]
    [SerializeField, ReadOnly] private Transform transform;
    [SerializeField, ReadOnly] private float speed;
    [SerializeField, ReadOnly] private float power;
    [SerializeField, ReadOnly] private float damping;

    [System.Serializable]
    public struct BodyData {
        public float speed;
        public float power;
        public float damping;
    }

    public Body(Transform transform, BodyData bodyData, bool useGravity = true) {
        this.transform = transform;
        this.target = transform.position;
        this.speed = bodyData.speed;
        this.power = bodyData.power;
        this.damping = bodyData.damping;
        this.useGravity = useGravity;
        isEnabled = true;
    }

    #region COMMANDS
    // Stops the body.
    public void Stop() {
        target = transform.position;
    }

    // Moves the body to this position.
    public void MoveTo(Vector3 position) {
        target = position;
        target.z = 0f;
    }
    #endregion

    // Updates the body.
    public void Update(float deltaTime) {

        if (!isEnabled) {
            return;
        }

        // Get the direction.
        Vector3 direction = target - transform.position;
        if (direction.sqrMagnitude < GameRules.MovementPrecision) {
            direction = Vector3.zero;
        }
        direction = direction.normalized;

        // Acceleration.
        Vector3 acceleration = direction * power;
        velocity += acceleration * deltaTime;
        velocity = speed < velocity.magnitude ? speed * velocity.normalized : velocity;

        // Damping.
        if (acceleration == Vector3.zero) {
            velocity *= damping;
        }

        // Moving.
        transform.position += velocity * deltaTime;

        if (useGravity) {
            Gravity(deltaTime);
            target = transform.position;
        }

    }

    public float feetWidth = 0.35f;
    public float bodyWidth = 0.35f;

    // Affects the body with gravity.
    private void Gravity(float deltaTime) {

        Vector3 position = transform.position;

        // temp floor.
        Vector2 feetPosition = (Vector2)position + Vector2.down * bodyWidth;
        Collider2D[] hits = Physics2D.OverlapBoxAll(feetPosition, new Vector2(feetWidth, 0.05f), 0f);
        Debug.DrawLine((Vector3)feetPosition - Vector3.right * feetWidth, (Vector3)feetPosition + Vector3.right * feetWidth);

        Debug.Log(hits.Length);
        bool b_Floor = false;
        for (int i = 0; i < hits.Length; i++) {
            Floor floor = hits[i].GetComponent<Floor>();
            if (floor != null) {
                b_Floor = true;
                break;
            }
        }
        Debug.Log(b_Floor);

        if (position.y <= -5f) {
            position.y = 0f;
            transform.position = position;
            return;
        }

        Vector3 gravity = new Vector3(0f, GameRules.Gravity, 0f);
        transform.position += gravity * deltaTime;

    }

}
