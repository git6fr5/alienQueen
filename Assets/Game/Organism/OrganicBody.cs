// Libraries.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrganicBody : MonoBehaviour {

    #region Parameters
    [Space(2), Header("Dimensions")]
    [SerializeField] protected float width = 0f; // The width of the body.
    [SerializeField] protected float height = 0f; // The height of the body.
    [Space(2), Header("Stats")]
    [SerializeField] private float maxSpeed = 0f; // The maximum speed of this organism.
    [SerializeField] private float force = 0f; // The force with which this organism moves.
    [SerializeField] private float jumpForce = 0f; // The force with which this jumps.
    [Space(2), Header("Debugging")]
    [SerializeField] private bool debugSize = false;
    #endregion

    #region Properties
    [Space(2), Header("Controls")]
    [SerializeField, ReadOnly] private Vector3 target = Vector3.zero; // The current target that the body is moving towards.
    [SerializeField, ReadOnly] private Vector3 velocity = Vector3.zero; // The current velocity of this organism.
    [SerializeField, ReadOnly] private Vector3 acceleration = Vector3.zero; // The current acceleration of this organism.
    [Space(2), Header("Gravity")]
    [SerializeField, ReadOnly] private Vector3 gravity = Vector3.zero; // The current gravitational acceleration on this organism.
    [SerializeField, ReadOnly] private bool useGravity = false; // Switch to enable gravitational interaction.
    [SerializeField, ReadOnly] private float jumpTicks = 0f; // Tracks how long the body has been without contacting the floor.
    [Space(2), Header("Knockback")]
    [SerializeField, ReadOnly] private Vector3 knockback = Vector3.zero; // The current knockback acting on this body.
    [SerializeField, ReadOnly] private bool isKnockbacked = false; // Switch that tracks whehter this is being knocked back.
    [SerializeField, ReadOnly] private float knockbackDuration = 0f; // The duration for which this body will be knocked back.
    [SerializeField, ReadOnly] private float knockbackTicks = 0f; // Tracks how long the body has been knocked back for.
    [Space(2), Header("Collision")]
    [SerializeField, ReadOnly] public bool facingRight = false; // Switch that tracks whether this is on the floor;
    [SerializeField, ReadOnly] public bool collisionForward = false; // Switch that tracks whether this is on the floor;
    [SerializeField, ReadOnly] public bool collisionFloor = false; // Switch that tracks whether this is on the floor;
    [SerializeField, ReadOnly] public bool collisionCeiling = false; // Switch that tracks whether this is on the floor;
    #endregion

    #region Unity
    // Runs once before the first frame.
    private void Start() {
        Init();
    }

    // Runs once every frame.
    private void Update() {

        // Cache the time differential.
        float deltaTime = Time.deltaTime;

        if (isKnockbacked) {
            Knockback(deltaTime);
            return;
        }
        else {
            Move(deltaTime);
            Gravity(deltaTime);
        }

    }

    // Runs every draw call.
    private void OnDrawGizmos() {
        // Debugging the collision box.
        Gizmos.color = Color.green;
        if (debugSize) {
            Gizmos.DrawWireCube(transform.position, new Vector3(width, height, 1));
        }

    }
    #endregion

    #region Commands
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
    public void AddKnockback(Vector3 knockback, float magnitude, float duration) {
        this.knockback = magnitude * knockback.normalized;
        knockbackDuration = Mathf.Max(knockbackDuration - knockbackTicks, duration);
        knockbackTicks = 0f;
        isKnockbacked = true;
    }

    public void Jump() {
        if (jumpTicks == 0f) {
            gravity += Vector3.up * jumpForce;
        }
    }
    #endregion

    #region Methods
    // Run to initialize the script.
    private void Init() {
        target = transform.position;
    }

    // Moves the body.
    private void Move(float deltaTime) {

        // Get the direction.
        Vector3 direction = target - transform.position;
        if (direction.sqrMagnitude < GameRules.MovementPrecision * GameRules.MovementPrecision) {
            direction = Vector3.zero;
        }
        direction = direction.normalized;

        // temp
        bool attachedA = CheckSides(transform.position + Vector3.right * GameRules.MovementPrecision, true);
        bool attachedB = CheckSides(transform.position - Vector3.right * GameRules.MovementPrecision, false);
        if (attachedA || attachedB) {
        }
        else if (direction.y > 0f) {
            direction.y = 0f;
        }

        // Acceleration.
        acceleration = direction * force;
        velocity += acceleration * deltaTime;
        velocity = maxSpeed < velocity.magnitude ? maxSpeed * velocity.normalized : velocity;

        // Damping.
        if (acceleration == Vector3.zero) {
            velocity *= GameRules.VelocityDamping;
            if (velocity.sqrMagnitude < GameRules.MovementPrecision * GameRules.MovementPrecision) {
                velocity = Vector3.zero;
            }

        }

        int n = (int)Mathf.Ceil(velocity.magnitude / GameRules.MovementPrecision);
        for (int i = 0; i < n; i++) {
            Vector3 position = transform.position + velocity / (float)n * deltaTime;
            facingRight = CheckDirection();
            collisionForward = CheckSides(position, facingRight);
            collisionFloor = CheckFeet(position);
            collisionCeiling = CheckFeet(position, false);

            if (collisionFloor && velocity.y < 0) {
                velocity.y = 0f;
            }
            else if (collisionCeiling && velocity.y > 0) {
                velocity.y = 0f;
            }

            // Moving.
            if (!collisionForward) {
                transform.position += velocity / (float)n * deltaTime;
            }
            else {
                velocity.x = 0f;
            }
        }
        
    }

    // Affects the body with gravity.
    private void Gravity(float deltaTime) {

        useGravity = CheckGravity();
        if (!useGravity) {
            gravity = Vector3.zero;
            return;
        }

        gravity += Vector3.up * GameRules.Gravity * deltaTime;

        int n = (int)Mathf.Ceil(gravity.magnitude / GameRules.MovementPrecision);
        for (int i = 0; i < n; i++) {

            Vector3 position = transform.position;
            collisionFloor = CheckFeet(position);
            collisionCeiling = CheckFeet(position, false);

            if (collisionCeiling && gravity.y > 0) {
                gravity.y = 0f;
            }
            if (collisionFloor) {
                if (gravity.y < 0) {
                    gravity.y = 0f;
                }
                jumpTicks = 0f;
            }
            else {
                jumpTicks += deltaTime;
            }
            transform.position += gravity / n * deltaTime;

        }

        target.y = transform.position.y;

    }

    // Affects the body with knockback.
    private void Knockback(float deltaTime) {
        knockbackTicks += deltaTime;
        if (knockbackTicks > knockbackDuration) {
            knockbackDuration = 0f;
            knockbackTicks = 0f;
            isKnockbacked = false;
        }
        transform.position += knockback * deltaTime;
    }
    #endregion

    #region Collision
    // Checks whether the body is contacting on the top or bottom.
    protected bool CheckFeet(Vector3 position, bool bottom = true) {
        float flip = bottom ? 1f : -1f;
        Vector3 collisionPosition = position + flip * Vector3.down * height / 2f;
        RaycastHit2D[] hits = Physics2D.LinecastAll(collisionPosition - Vector3.right * (width / 2f - GameRules.MovementPrecision),
            collisionPosition + Vector3.right * (width / 2f - GameRules.MovementPrecision));

        bool collision = false;
        for (int i = 0; i < hits.Length; i++) {
            Floor floor = hits[i].collider.GetComponent<Floor>();
            if (floor != null) {
                collision = true;
                break;
            }
        }

        return collision;
    }

    // Checks whether the body is contacting on either side.
    protected bool CheckSides(Vector3 position, bool forward) {
        float flip = forward ? 1f : -1f;
        Vector3 collisionPosition = position + flip * Vector3.right * width / 2f;
        RaycastHit2D[] hits = Physics2D.LinecastAll(collisionPosition - Vector3.down * (height / 2f - GameRules.MovementPrecision), 
            collisionPosition + Vector3.down * (height / 2f - GameRules.MovementPrecision));

        bool collision = false;
        for (int i = 0; i < hits.Length; i++) {
            Floor floor = hits[i].collider.GetComponent<Floor>();
            if (floor != null) {
                collision = true;
                break;
            }
        }

        return collision;
    }

    // Checks which direction the body is moving in.
    protected bool CheckDirection() {
        return velocity.x > 0f;
    }

    // Checks whether this body is using gravity.
    protected virtual bool CheckGravity() {
        Vector3 position = transform.position;
        return !CheckFeet(position);
    }
    #endregion

}
