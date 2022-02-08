/* --- Libraries --- */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the actions of a character.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class Controller : MonoBehaviour {

    /* --- Enumerations --- */
    // Direction States
    public enum Direction {
        Right,
        Left
    }
    // Movement States
    public enum Movement {
        Idle,
        Moving,
        Dashing,
    }
    // Falling States
    public enum Airborne {
        Grounded,
        Rising,
        Falling
    }

    /* --- Components --- */
    [HideInInspector] public Rigidbody2D body; // Handles physics calculations.
    [SerializeField] protected Mesh mesh; // Handles the collision frame and animation.

    /* --- Parameters --- */
    [Space(2), Header("Parameters")]
    [SerializeField, Range(0, 20)] protected float baseSpeed; // The base speed at which this character moves.
    [SerializeField, Range(0, 100)] protected float baseAcceleration; // The base acceleration at which this character moves.
    [SerializeField, Range(0, 100)] protected float baseWeight; // The base weight that the character has.
    [SerializeField, Range(0, 100)] public float baseJump; // The base weight that the character has.
    [SerializeField, Range(0.05f, 1f)] public float floatiness = 0.4f; // The character's floating weight

    /* --- Properties --- */
    [Space(2), Header("Properties")]
    [SerializeField, ReadOnly] protected float moveSpeed; // The character's movement speed.
    [SerializeField, ReadOnly] protected float moveDirection; // The direction the character is moving.
    [SerializeField, ReadOnly] protected float weight; // The character's movement speed.

    /* --- Switches --- */
    [Space(2), Header("Switches")]
    [SerializeField, ReadOnly] public bool think; // Whether this character is in control.
    [SerializeField, ReadOnly] public bool jump; // Whether this character should jump.
    [SerializeField, ReadOnly] public bool action; // Whether this character should perform an action.
    [SerializeField, ReadOnly] public bool die; // Whether this character should perform an action.

    /* --- Flags --- */
    [Space(2), Header("Flags")]
    [SerializeField, ReadOnly] public Movement movementFlag = Movement.Idle; // Flags what type of movement this controller is in.
    [SerializeField, ReadOnly] public Direction directionFlag = Direction.Right; // Flags what type of movement this controller is in.
    [SerializeField, ReadOnly] public Airborne airborneFlag = Airborne.Grounded; // Flags what type of movement this controller is in

    /* --- Unity --- */
    // Runs once before the first frame.
    private void Start() {
        Init();
    }

    // Runs once every frame.
    private void Update() {
        if (think) {
            Think();
        }
        Process();
        Flag();
    }

    // Runs once every fixed interval.
    private void FixedUpdate() {
        float deltaTime = Time.fixedDeltaTime;
        Move(deltaTime);
    }

    /* --- Virtual Methods --- */
    // Runs the initialization logic.
    protected virtual void Init() {
        // Cache these components.
        body = GetComponent<Rigidbody2D>();
        body.constraints = RigidbodyConstraints2D.FreezeRotation;
        // Set up these switches.
        think = true;
        jump = false;
        action = false;
        die = false;
    }

    // Runs the thinking logic.
    protected virtual void Think() {
        moveSpeed = baseSpeed;
        moveDirection = 0f;
        weight = baseWeight;
        if (!mesh.hurtbox.empty) {
            die = true;
        }
    }

    /* --- Methods --- */
    // Processes any events.
    private void Process() {
        if (jump) {
            Jump();
            jump = false;
        }
        if (action) {
            Action();
            action = false;
        }
        if (die) {
            Die();
            die = false;
        }
    }

    // Checks relevant state flags for this controller.
    private void Flag() {
        DirectionFlag();
        MovementFlag();
        AirborneFlag();
    }

    // Moves this controller based on it's input.
    private void Move(float deltaTime) {
        float targetVelocity = moveSpeed * moveDirection;
        if (Mathf.Abs(targetVelocity - body.velocity.x) >= baseAcceleration * deltaTime) {
            float deltaVelocity = Mathf.Sign(targetVelocity - body.velocity.x) * baseAcceleration * deltaTime;
            body.velocity = new Vector2(body.velocity.x + deltaVelocity, body.velocity.y);
        }
        else {
            body.velocity = new Vector2(targetVelocity, body.velocity.y);
        }
    }

    /* --- Events --- */
    private void Die() {
        //
    }

    // Performs a jump.
    private void Jump() {
        if (!mesh.feetbox.empty) {
            body.velocity = new Vector2(body.velocity.x, body.velocity.y + baseJump);
        }
    }

    /* --- Virtual Events --- */
    // Performs an action.
    protected virtual void Action() {
        // Determined by the particular type of controller.
    }

    /* --- Flag Methods --- */
    // Flags which direction the controller is facing.
    private void DirectionFlag() {
        if (moveDirection != 0) {
            directionFlag = (moveDirection > 0) ? Direction.Right : Direction.Left;
        }
    }

    // Flags whether this controller is moving.
    private void MovementFlag() {
        movementFlag = Movement.Idle;
        if (moveDirection != 0 && moveSpeed != 0) {
            movementFlag = Movement.Moving;
        }
    }

    // Flags whether this controller is airborne.
    private void AirborneFlag() {
        body.gravityScale = weight * GameRules.GravityScale;
        if (mesh.feetbox.empty) {
            airborneFlag = Airborne.Rising;
            if (body.velocity.y < 0) {
                airborneFlag = Airborne.Falling;
            }
        }
        else {
            airborneFlag = Airborne.Grounded;
        }
    }

}
