using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public Queen queen;
    public Alien alien;
    public bool mouseInput;
    public bool keyInput;

    void Start() {
        if (queen != null) {
            queen.Init(this);
        }
    }

    void Update() {
        if (queen != null) {
            queen.OnUpdate(Time.deltaTime, Input.GetMouseButtonDown(0));
        }
        Control();
    }

    private bool Control() {

        Alien alien = GetAlien();
        if (this.alien != null) {
            alien = this.alien;
        }

        bool b_CanControl = alien != null;
        if (!b_CanControl) {
            return false;
        }

        bool b_Input = false;
        if (mouseInput) {
            b_Input = Input.GetMouseButtonDown(1);
            if (b_Input) {
                Vector3 mousePosition = GameRules.MousePosition;
                alien.body.MoveTo(mousePosition);
            }
        }

        if (keyInput) {
            float horizontal = Input.GetAxisRaw("Horizontal");
            bool jump = Input.GetKeyDown(KeyCode.Space);
            bool action = Input.GetKeyDown(KeyCode.P);
            b_Input = horizontal != 0f || jump || action;
            alien.body.MoveTo(alien.transform.position + horizontal * Vector3.right);
            if (jump) {
                alien.body.Jump();
            }
            if (action) {
                alien.Action();
            }
        }

        return b_Input;
    }

    private Alien GetAlien() {
        return queen?.nest.selectedAlien;
    }

}
