/* --- Libraries --- */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class Rabbit : Controller {

    /* --- Parameters --- */
    [Space(2), Header("Action")]
    [SerializeField] [Range(0f, 40f)] protected float hopSpeed = 20f;

    /* --- Properties --- */
    [SerializeField, ReadOnly] public bool canHop;
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
            if (Input.GetKey(actionKey)) {
                action = true;
            }
        }
        if (Input.GetKey(jumpKey) && airborneFlag == Airborne.Rising) {
            weight *= floatiness;
        }

        // Get the action.
        if (!mesh.feetbox.empty) {
            canHop = true;
        }

    }

    /* --- Overridden Events --- */
    // Performs an action.
    protected override void Action() {
        base.Action(); // Runs the base action.

        if (canHop) {
            body.velocity = new Vector2(body.velocity.x, body.velocity.y + hopSpeed);
            canHop = false;
        }

    }

}
