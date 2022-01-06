using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Replete : Alien {

    // Initializes the alien.
    public override void Init(Queen queen) {
        isControllable = true;
        base.Init(queen);
    }

    public override void Attack() {
        isControllable = false;
        speed = new GameRules.Params(0f, 0f);
    }

    public void TakeBiomass(Alien alien) {
        float neededMass = alien.maxBiomass - alien.biomass;
        if (neededMass > biomass) {
            alien.biomass += neededMass;
            biomass -= neededMass;
        }
        else {
            alien.biomass += biomass;
            biomass = 0f;
        }
    }

    public void StoreBiomass(Alien alien) {
        biomass += alien.biomass;
        if (biomass > maxBiomass) {
            alien.biomass = biomass - maxBiomass;
            biomass = maxBiomass;
        }
        else {
            alien.biomass = 0f;
        }
    }

}
