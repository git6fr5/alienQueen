using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Queen : Organism {

    // Components.
    Player player;
    SpriteRenderer spriteRenderer;
    public QueenUI queenUI;

    // Properties.
    [Space(5), Header("Switches")]
    [SerializeField] public bool initialize;

    // Incubator.
    [Space(5), Header("Incubator")]
    [SerializeField] public Incubator incubator;
    [SerializeField] public Incubator.IncubatorData incubatorData;

    // Nest.
    [Space(5), Header("Nest")]
    [SerializeField] public Nest nest;
    [SerializeField] private float hatchRadius = 0f;

    // Selection.
    [Space(5), Header("Selection")]
    [SerializeField] public Selector selector;
    [SerializeField] private float selectionRadius = 0f;

    public void Init(Player player) {

        incubator = new Incubator(this, incubatorData);
        nest = new Nest(hatchRadius);
        selector = new Selector(transform, selectionRadius);

        spriteRenderer = GetComponent<SpriteRenderer>();

        initialize = false;
        this.player = player;

        if (queenUI != null) {
            queenUI.Init(this);
        }

        gameObject.SetActive(true);
    }

    private void Update() {
        if (initialize) {
            Init(null);
            initialize = false;
        }
        if (player == null) {
            OnUpdate(Time.deltaTime, Input.GetMouseButtonDown(0));
        }
    }

    public void OnUpdate(float deltaTime, bool clicked) {
        selector.Update(clicked);
        incubator.Update(deltaTime);
        nest.Update(deltaTime, clicked);
        if (queenUI != null) { queenUI.OnUpdate(); }
        Render();
    }

    void Render() {
        float f_OutlineWidth = selector.isSelected ? GameRules.OutlineWidth : 0f;
        spriteRenderer.material.SetFloat("_OutlineWidth", f_OutlineWidth); 
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, selectionRadius);
    }

}
