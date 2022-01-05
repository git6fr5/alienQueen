using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class EggUI : MonoBehaviour {

    // Components.
    SpriteRenderer spriteRenderer;
    QueenUI queenUI;

    // Properties.
    public int index;
    public bool isSelected;
    public bool isCancelled;
    public bool isMouseOver;

    public void Init(QueenUI queenUI, int index, Transform parent) {
        this.queenUI = queenUI;
        this.index = index;

        float angle = 360f * (float)index / (float)queenUI.queen.eggs.Length;
        transform.position = queenUI.transform.position + queenUI.optionRadius * (Quaternion.Euler(0f, 0f, angle) * Vector3.right);
        transform.SetParent(parent);

        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = queenUI.queen.eggs[index].eggSprite;
    }

    void Update() {
        CheckSelect();
        if (isSelected) {
            queenUI.queen.AddToQueue(index);
            isSelected = false;
        }
        if (isCancelled) {
            queenUI.queen.RemoveFromQueue(index);
            isCancelled = false;
        }
    }

    void OnMouseOver() {
        isMouseOver = true;
    }

    void OnMouseExit() {
        isMouseOver = false;
    }


    void CheckSelect() {
        if (Input.GetMouseButtonDown(0)) {
            if (Input.GetKey(KeyCode.LeftShift)) {
                isCancelled = isMouseOver;
            }
            else {
                isSelected = isMouseOver;
            }
        }
    }


}
