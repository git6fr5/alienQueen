/* --- Libraries --- */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class Hurtbox : Container {

    /* --- Parameters --- */
    [SerializeField] private string enemy = "";

    // Initializes the script.
    protected override void Init() {
        base.Init(); // Runs the base initialization.
        target = enemy;
    }

}
