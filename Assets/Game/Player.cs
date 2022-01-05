using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public QueenUI queenUI;
    public Alien alien;

    void Update() {
        Select();
        Control();
    }

    void Select() {

        if (Input.GetMouseButtonDown(0)) {

            bool b_UISelection = false;
            if (queenUI.isMouseOver && !queenUI.isSelected) {
                // Queen
                queenUI.isSelected = true;
                b_UISelection = true;
            }
            else if (queenUI.isSelected) {
                // Eggs
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

            bool b_AlienSelection = false;
            if (!b_UISelection) {

                queenUI.isSelected = false;

                // Nest
                for (int i = 0; i < queenUI.queen.nest.Count; i++) {

                    if (queenUI.queen.nest[i].isMouseOver) {
                        // Deselect everything else.
                        for (int j = 0; j < queenUI.queen.nest.Count; j++) {
                            queenUI.queen.nest[j].isSelected = false;
                        }
                        queenUI.queen.nest[i].isSelected = true;
                        alien = queenUI.queen.nest[i];
                        b_AlienSelection = true; // Not really being used for anything right now.
                        break;
                    }

                }

            }

        }

        if (Input.GetMouseButtonDown(1)) {

            alien.isSelected = false;
            alien = null;
            queenUI.isSelected = false;

        }

    }

    void Control() {

        if (alien != null && queenUI.isSelected) {
            alien.isSelected = false;
        }
        else if (alien != null && !queenUI.isSelected) {
            alien.isSelected = true;
        }

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        bool attack = Input.GetKeyDown(KeyCode.Space);

        bool b_Input = (horizontal != 0 || vertical != 0f || attack);
        if (queenUI.isSelected && alien != null && b_Input) {
            queenUI.isSelected = false;
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
