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
    public Selector selector;
    public float selectionRadius = 0.35f;
    public int index;

    public void Init(QueenUI queenUI, int index, Transform parent) {
        this.queenUI = queenUI;
        this.index = index;

        float angle = 360f * (float)index / (float)queenUI.queen.incubator.eggs.Length;
        transform.position = queenUI.transform.position + queenUI.optionRadius * (Quaternion.Euler(0f, 0f, angle) * Vector3.right);
        transform.SetParent(parent);

        selector = new Selector(transform, selectionRadius);

        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = queenUI.queen.incubator.eggs[index].eggSprite;
    }

    void OnUpdate() {
        if (selector.isSelected) {
            queenUI.queen.incubator.AddToQueue(index);
            selector.isSelected = false;
        }
        //if (isCancelled) {
        //    queenUI.queen.incubator.RemoveFromQueue(index);
        //    isCancelled = false;
        //}
    }


}
