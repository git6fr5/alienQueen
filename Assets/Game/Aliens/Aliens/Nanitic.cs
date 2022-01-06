using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Can kill humans and eat their biomass.
/// Can store a small amount of biomass.
/// </summary>
public class Nanitic : Alien {

    // Initializes the alien.
    public override void Init(Queen queen) {
        isControllable = true;
        base.Init(queen);
    }

    public override void Attack() {

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, attackRadius);
        for (int i = 0; i < hits.Length; i++) {
            Human human = hits[i].GetComponent<Human>();
            if (human != null) {
                human.Hurt(1);
                return;
            }

            Biomass biomass = hits[i].GetComponent<Biomass>();
            if (biomass != null) {
                Eat(biomass);
                return;
            }

            Queen queen = hits[i].GetComponent<Queen>();
            if (queen != null) {
                queen.StoreBiomass(this);
                return;
            }

            Replete replete = hits[i].GetComponent<Replete>();
            if (replete != null && !replete.isControllable) {
                replete.StoreBiomass(this);
                return;
            }
        }
    }

}
