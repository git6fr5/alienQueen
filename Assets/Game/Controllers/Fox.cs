/* --- Libraries --- */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class Fox : Controller {

    /* --- Parameters --- */
    [Space(2), Header("Action")]
    [SerializeField] [Range(0f, 40f)] protected float dashSpeed = 20f;
    [SerializeField] [Range(0f, 0.5f)] protected float dashDuration = 0.1f;
    [HideInInspector] protected Coroutine dashTimer = null;

    /* --- Properties --- */
    [SerializeField, ReadOnly] public bool canDash;
    [SerializeField, ReadOnly] private KeyCode jumpKey = KeyCode.Space; // The key used to jump.
    [SerializeField, ReadOnly] private KeyCode actionKey = KeyCode.J; // The key used to perform the action.

    /* --- Overridden Methods --- */
    // Runs the thinking logic.
    protected override void Think() {
        base.Think(); // Runs the base think.

        // Get the movement.
        moveDirection = Input.GetAxisRaw("Horizontal");

        // Get the jump.
        if (Input.GetKeyDown(jumpKey)) {
            jump = true;
        }
        if (Input.GetKey(jumpKey) && airborneFlag == Airborne.Rising) {
            weight *= floatiness;
        }

        // Get the action.
        if (!mesh.feetbox.empty && dashTimer == null) {
            canDash = true;
        }
        if (Input.GetKeyDown(actionKey)) {
            action = true;
        }

    }

    /* --- Overridden Events --- */
    // Performs an action.
    protected override void Action() {
        base.Action(); // Runs the base action.

        // if (!canDash) { return; }

        if (dashTimer == null && canDash) {
            Vector2 dashVector = new Vector2(Input.GetAxisRaw("Horizontal"), 0f).normalized;
            body.velocity = dashVector * dashSpeed;
            print(body.velocity);
            weight = 0f;
            // print(body.gravityScale);
            think = false;
            canDash = false;
            dashTimer = StartCoroutine(IEDash(dashDuration));
        }

        IEnumerator IEDash(float delay) {
            yield return new WaitForSeconds(delay);
            think = true;
            yield return (dashTimer = null);
        }

    }

}
