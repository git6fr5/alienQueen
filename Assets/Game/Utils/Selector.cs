using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Selector {

    // Properties.
    [Space(5), Header("Internal Controls")]
    [SerializeField, ReadOnly] public bool isSelected;
    [SerializeField, ReadOnly] public bool isMouseOver;

    [Space(5), Header("Settings")]
    [SerializeField, ReadOnly] private Transform transform;
    [SerializeField, ReadOnly] private float radius;

    public Selector(Transform transform, float radius) {
        this.transform = transform;
        this.radius = radius;
        this.isMouseOver = false;
        this.isSelected = false;
    }

    public void Update(bool clicked) {
        isMouseOver = Check();
        if (clicked) {
            isSelected = isMouseOver;
        }
    }

    private bool Check() {
        // Check the position.
        Vector3 mousePosition = GameRules.MousePosition;
        mousePosition.z = 0f;
        float distance = (mousePosition - transform.position).sqrMagnitude;
        return distance < (radius * radius);
    }

}
