// Libraries.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Definitions.
using Params = GameRules.Params;

[RequireComponent(typeof(SpriteRenderer))]
public class Alien : MonoBehaviour {

    // Components.
    Queen queen;
    SpriteRenderer spriteRenderer;

    // Properties.
    [Space(5), Header("Switches")]
    [SerializeField] public bool initialize;

    [Space(5), Header("Properties")]
    [SerializeField] public Biomass biomass;
    [SerializeField] public Body.BodyData bodyData;
    [SerializeField] public float selectionRadius;

    [Space(5), Header("Modules")]
    [SerializeField] public Body body;
    [SerializeField] public Selector selector;

    // Initializes the alien.
    public virtual void Init(Queen queen) {
        if (queen != null) {
            transform.position = queen.transform.position + (Vector3)(Random.insideUnitCircle.normalized) * queen.nest.hatchRadius;
        }
        spriteRenderer = GetComponent<SpriteRenderer>();

        body = new Body(transform, bodyData);
        selector = new Selector(transform, selectionRadius);

        initialize = false;
        this.queen = queen;
        gameObject.SetActive(true);
    }

    private void Update() {
        if (initialize) {
            Init(null);
            initialize = false;
        }
        if (queen == null) {
            OnUpdate(Time.deltaTime, Input.GetMouseButtonDown(0));
        }
    }

    // Runs once per frame.
    public void OnUpdate(float deltaTime, bool clicked) {
        selector.Update(clicked);
        body.Update(deltaTime);
        Render();
    }

    void Render() {
        float f_OutlineWidth = selector.isSelected ? GameRules.OutlineWidth : 0f;
        spriteRenderer.material.SetFloat("_OutlineWidth", f_OutlineWidth);
    }


}
