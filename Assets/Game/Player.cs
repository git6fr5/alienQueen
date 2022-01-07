using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public Queen queen;
    public bool mouseInput;
    public bool keyInput;

    void Start() {
        queen.Init(this);
    }

    void Update() {
        queen.OnUpdate(Time.deltaTime, Input.GetMouseButtonDown(0));
        Control();
    }

    private bool Control() {

        Alien alien = GetAlien();

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
            b_Input = horizontal != 0f;
            alien.body.MoveTo(alien.transform.position + horizontal * Vector3.right);
        }

        return b_Input;
    }

    private Alien GetAlien() {
        return queen.nest.selectedAlien;
    }

}
