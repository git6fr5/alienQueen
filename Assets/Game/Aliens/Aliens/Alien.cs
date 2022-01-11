// Libraries.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Definitions.
using Params = GameRules.Params;

public class Alien : Organism {

    // Components.
    Queen queen;
    public AlienUI alienUI;

    // Properties.
    [Space(5), Header("Switches")]
    [SerializeField] public bool initialize;

    [Space(5), Header("Properties")]
    [SerializeField] public Biomass biomass;
    [SerializeField] public Body.BodyData bodyData;

    [Space(5), Header("Modules")]
    [SerializeField] public Body body;
    [SerializeField] public Selector selector;

    // Initializes the alien.
    public virtual void Init(Queen queen) {
        if (queen != null) {
            transform.position = queen.transform.position + (Vector3)(Random.insideUnitCircle.normalized) * queen.nest.hatchRadius;
        }

        body = new Body(transform, bodyData);
        selector = new Selector(transform, Mathf.Max(bodyData.length, bodyData.width));

        if (alienUI != null) {
            alienUI.Init(this);
        }

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
        if (alienUI != null) { alienUI.OnUpdate(); }
    }

    public void Action() {
        print("Performing Alien Action");
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 1f);
        for (int i = 0; i < hits.Length; i++) {
            Human human = hits[i].GetComponent<Human>();
            if (human != null) {
                human.OnHurt(1, Vector3.right * -(transform.position.x - human.transform.position.x), 8f, 0.25f);
            }
        }

    }


}
