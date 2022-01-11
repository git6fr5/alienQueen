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
    [SerializeField, ReadOnly] public Vector3 velocity;
    [SerializeField, ReadOnly] public Vector3 acceleration;

    [Space(5), Header("Settings")]
    [SerializeField, ReadOnly] private Transform transform;
    [SerializeField, ReadOnly] private float speed;
    [SerializeField, ReadOnly] private float power;
    [SerializeField, ReadOnly] private float damping;
    [SerializeField, ReadOnly] public float length;
    [SerializeField, ReadOnly] public float width;
    [SerializeField, ReadOnly] private float jumpForce;
    [SerializeField, ReadOnly] private float jumpTicks = 0f;
    [SerializeField, ReadOnly] private Vector3 gravity;

    [Space(5), Header("Knockback")]
    [SerializeField, ReadOnly] private bool isKnockedback;
    [SerializeField, ReadOnly] private float knockbackDuration;
    [SerializeField, ReadOnly] private float knockbackTicks;
    [SerializeField, ReadOnly] private Vector3 knockbackVector;


    [System.Serializable]
    public struct BodyData {
        public float speed;
        public float power;
        public float damping;
        public float length;
        public float width;
        public float jumpForce;
    }

    public Body(Transform transform, BodyData bodyData, bool useGravity = true) {
        this.transform = transform;
        this.target = transform.position;
        this.speed = bodyData.speed;
        this.power = bodyData.power;
        this.damping = bodyData.damping;
        this.length = bodyData.length;
        this.width = bodyData.width;
        this.jumpForce = bodyData.jumpForce;
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

    // Moves the body to this position.
    public void AddKnockback(Vector3 knockbackDirection, float magnitude, float duration) {
        knockbackVector = magnitude * knockbackDirection.normalized;
        knockbackDuration = Mathf.Max(knockbackDuration - knockbackTicks, duration);
        knockbackTicks = 0f;
        isKnockedback = true;
    }

    public void Jump() {
        Debug.Log("Trying to jump");
        if (jumpTicks == 0f) {
            gravity += Vector3.up * jumpForce;
        }
    }
    #endregion

    // Updates the body.
    public void Update(float deltaTime) {

        if (!isEnabled) {
            return;
        }

        if (isKnockedback) {
            Knockback(deltaTime);
            return;
        }

        // Get the direction.
        Vector3 direction = target - transform.position;
        if (direction.sqrMagnitude < GameRules.MovementPrecision * GameRules.MovementPrecision) {
            direction = Vector3.zero;
        }
        direction = direction.normalized;

        // Acceleration.
        acceleration = direction * power;
        velocity += acceleration * deltaTime;
        velocity = speed < velocity.magnitude ? speed * velocity.normalized : velocity;

        // Damping.
        if (acceleration == Vector3.zero) {
            velocity *= damping;
            if (velocity.sqrMagnitude < GameRules.MovementPrecision * GameRules.MovementPrecision) {
                velocity = Vector3.zero;
            }

        }

        Move(deltaTime);

        if (useGravity) {
            Gravity(deltaTime);
            target.y = transform.position.y;
        }

    }

    private void Move(float deltaTime) {
        Vector3 position = transform.position;
        bool b_Forward = velocity.x > 0f;
        bool b_Blocked = CheckSides(position, b_Forward);
    
        // Moving.
        if (!b_Blocked) {
            transform.position += velocity * deltaTime;
        }
    }

    private void Knockback(float deltaTime) {
        knockbackTicks += deltaTime;
        if (knockbackTicks > knockbackDuration) {
            knockbackDuration = 0f;
            knockbackTicks = 0f;
            isKnockedback = false;
        }
        transform.position += knockbackVector * deltaTime;
    }

    // Affects the body with gravity.
    private void Gravity(float deltaTime) {

        Vector3 position = transform.position;
        bool b_Floor = CheckFeet(position);
        bool b_Ceiling = CheckFeet(position, false);

        gravity += Vector3.up * GameRules.Gravity * deltaTime;
        if (b_Ceiling && gravity.y > 0) {
            gravity.y = 0f;
        }
        if (b_Floor) {
            if (gravity.y < 0) {
                gravity.y = 0f;
            }
            jumpTicks = 0f;
        }
        else {
            jumpTicks += deltaTime;
        }
        transform.position += gravity * deltaTime;

    }

    private bool CheckFeet(Vector3 position, bool bottom = true) {
        float flip = bottom ? 1f : -1f;
        Vector3 feetPosition = position + flip * Vector3.down * width;
        RaycastHit2D[] hits = Physics2D.LinecastAll(feetPosition - Vector3.right * length / 2f, feetPosition + Vector3.right * length / 2f);
        Debug.DrawLine(feetPosition - Vector3.right * length / 2f, feetPosition + Vector3.right * length / 2f);

        bool b_Floor = false;
        for (int i = 0; i < hits.Length; i++) {
            Floor floor = hits[i].collider.GetComponent<Floor>();
            if (floor != null) {
                b_Floor = true;
                break;
            }
        }

        return b_Floor;
    }

    private bool CheckSides(Vector3 position, bool forward) {
        float flip = forward ? 1f : -1f;
        Vector3 sidePosition = position + flip * Vector3.right * length;
        RaycastHit2D[] hits = Physics2D.LinecastAll(sidePosition - Vector3.down * width / 2f, sidePosition + Vector3.down * width / 2f);
        Debug.DrawLine(sidePosition - Vector3.down * width / 2f, sidePosition + Vector3.down * width / 2f);

        bool b_Blocked = false;
        for (int i = 0; i < hits.Length; i++) {
            Floor floor = hits[i].collider.GetComponent<Floor>();
            if (floor != null) {
                b_Blocked = true;
                break;
            }
        }

        return b_Blocked;
    }
}
