using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Look for biomass to collect from either the ground or from repletes.
/// Store their biomass in the queen.
/// </summary>
public class Drone : Alien {

    private bool isIdle = false;

    // Initializes the alien.
    public override void Init(Queen queen) {
        base.Init(queen);
        isControllable = false;
        StartCoroutine(IEIdle());
    }

    protected override void Think() {

        // Look for biomass to collect.
        bool b_FoundBiomass = false; 
        b_FoundBiomass = FindBiomass();
        if (!b_FoundBiomass) {
            b_FoundBiomass = FindReplete();
        }

        // Otherwise look for a place to store.
        bool b_FoundQueen = false;
        if (!b_FoundBiomass) {
            b_FoundQueen = FindQueen();
        }
        isIdle = (b_FoundBiomass || b_FoundQueen);
    }

    private IEnumerator IEIdle() {
        while (true) {
            if (!isIdle) {
                horizontal = Mathf.Sign(Random.Range(-1f, 1f));
                vertical = Mathf.Sign(Random.Range(-1f, 1f));
                attack = false;
            }
            yield return new WaitForSeconds(1f);
        }
    }

    private bool FindBiomass() {

        // The amount of biomass that can still be collected.
        float biomassLeft = maxBiomass - biomass;
        if (biomassLeft <= 0) {
            return false;
        }

        // Look for the nearest biomass.
        Biomass[] biomasses = (Biomass[])GameObject.FindObjectsOfType(typeof(Biomass));
        Biomass closestBiomass = null;
        float sqrDistance = Mathf.Infinity;

        for (int i = 0; i < biomasses.Length; i++) {
            // Check the biomass is targettable.
            bool b_BiomassRequirements = biomasses[i].value < biomassLeft;
            // Check the distance.
            float newSquareDistance = (transform.position - biomasses[i].transform.position).sqrMagnitude;
            if (b_BiomassRequirements && newSquareDistance < sqrDistance) {
                closestBiomass = biomasses[i];
                sqrDistance = newSquareDistance;
            }
        }

        // If a biomass was found.
        if (closestBiomass != null) {
            Vector3 target = closestBiomass.transform.position - transform.position;
            horizontal = Mathf.Sign(target.x);
            vertical = Mathf.Sign(target.y);
            if (target.sqrMagnitude < attackRadius * attackRadius) {
                attack = true;
            }
        }
        return (closestBiomass != null);
    }

    private bool FindQueen() {

        // Only if we have biomass to store.
        if (biomass <= 0f) {
            return false;
        }

        // Check if the queen has biomass storage available.
        Queen queen = (Queen)GameObject.FindObjectOfType(typeof(Queen));
        if (queen.biomass >= maxBiomass) {
            return false;
        }

        if (queen != null) {
            Vector3 target = queen.transform.position - transform.position;
            horizontal = Mathf.Sign(target.x);
            vertical = Mathf.Sign(target.y);
            if (target.sqrMagnitude < attackRadius * attackRadius) {
                attack = true;
            }
        }
        return (queen != null);
    }

    // Find a replete to take biomass from it and store it in the queen.
    bool FindReplete() {

        // Only if we have space to carry biomass.
        if (biomass >= maxBiomass) {
            return false;
        }

        // Only if we have biomass to store.
        Replete[] repletes = (Replete[])GameObject.FindObjectsOfType(typeof(Replete));
        Replete closestReplete = null;
        float sqrDistance = Mathf.Infinity;
        for (int i = 0; i < repletes.Length; i++) {
            // Check the replete is targettable.
            bool b_RepleteRequirements = !repletes[i].isControllable && repletes[i].biomass > 0f;
            // Check the distance.
            float newSquareDistance = (transform.position - repletes[i].transform.position).sqrMagnitude;
            if (b_RepleteRequirements && newSquareDistance < sqrDistance) {
                closestReplete = repletes[i];
                sqrDistance = newSquareDistance;
            }
        }

        // If a replete was found.
        if (closestReplete != null) {
            Vector3 target = closestReplete.transform.position - transform.position;
            horizontal = Mathf.Sign(target.x);
            vertical = Mathf.Sign(target.y);
            if (sqrDistance < attackRadius * attackRadius) {
                attack = true;
            }
        }
        return (closestReplete != null);
    }

    public override void Attack() {

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, attackRadius);
        for (int i = 0; i < hits.Length; i++) {

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
            if (replete != null) {
                replete.TakeBiomass(this);
                return;
            }
        }
    }

}
