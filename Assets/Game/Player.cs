using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public Queen queen;
    public Alien alien;

    void Update() {

        if (Input.GetMouseButtonDown(0)) {
            Select();
        }

        Control();
    }

    void Select() {
        bool b_UISelection = false;
        b_UISelection = UISelect(b_UISelection);

        bool b_AlienSelection = false;
        if (!b_UISelection) {
            b_AlienSelection = AlienSelect(b_AlienSelection);
        }

        if (!b_UISelection || !b_AlienSelection) {
            Deselect();
        }
    }

    void Deselect() {
        alien.isSelected = false;
        queen.isSelected = false;

        alien.Stop(alien.transform.position);
        alien = null;
    }

    bool AlienSelect(bool b_AlienSelection) {
        queen.isSelected = false;

        // Nest
        for (int i = 0; i < queen.nest.Count; i++) {

            if (queen.nest[i].isMouseOver) {
                // Deselect everything else.
                for (int j = 0; j < queen.nest.Count; j++) {
                    queen.nest[j].isSelected = false;
                }
                queen.nest[i].isSelected = true;
                alien = queen.nest[i];
                b_AlienSelection = true; // Not really being used for anything right now.
                break;
            }

        }

        return b_AlienSelection;
    }

    bool UISelect(bool b_UISelection) {
        if (queen.isMouseOver && !queen.isSelected) {
            // Queen
            queen.isSelected = true;
            b_UISelection = true;
        }
        else if (queen.isSelected) {
            // Eggs
            QueenUI queenUI = (QueenUI)GameObject.FindObjectOfType(typeof(QueenUI));
            for (int i = 0; i < queenUI.options.Count; i++) {

                if (queenUI.options[i].isMouseOver) {
                    if (Input.GetKey(KeyCode.LeftShift)) {
                        queenUI.options[i].isCancelled = true;
                    }
                    else {
                        queenUI.options[i].isSelected = true;
                    }
                    b_UISelection = true;
                    break;
                }

            }

        }

        return b_UISelection;
    }

    void Control() {

        if (alien != null && queen.isSelected) {
            alien.isSelected = false;
        }
        else if (alien != null && !queen.isSelected) {
            alien.isSelected = true;
        }

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        bool attack = Input.GetKeyDown(KeyCode.Space);
        if (Input.GetMouseButtonDown(1)) {
            Vector3 mousePos = GameRules.MousePosition;
            alien.MoveTo(mousePos);
        }

        bool b_Input = (horizontal != 0 || vertical != 0f || attack);
        if (queen.isSelected && alien != null && b_Input) {
            queen.isSelected = false;
            alien.isSelected = true;
        }

        if (alien == null || !alien.isSelected) {
            return;
        }

        alien.horizontal = horizontal;
        alien.vertical = vertical;
        alien.attack = attack;

    }

}
