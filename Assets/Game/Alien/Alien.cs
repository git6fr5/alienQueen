using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(AlienBody))]
public class Alien : Organism {

    public float visionRadius = 3f;
    public float attackRadius = 1f;

    // temp.
    public int index;
    float pingCooldown;

    void Update() {

        ProcessPing();

    }

    private void ProcessPing() {

        // Don't ping if the player is selected on this alien.
        if (GameRules.MainPlayer.organismIndex == index) {
            // return;
        }

        // Don't ping if this alien is hidden.
        AlienBody alienBody = organicBody.GetComponent<AlienBody>();
        if (alienBody != null && alienBody.isHidden) {
            return;
        }

        // Don't ping if the ping is on cooldown.
        pingCooldown -= Time.deltaTime;
        if (pingCooldown > 0) {
            return;
        }

        // Check if a human is close by.
        pingCooldown = 0f;
        Human[] humans = (Human[])GameObject.FindObjectsOfType(typeof(Human)); // Find a better way to do this.
        for (int i = 0; i < humans.Length; i++) {
            if ((humans[i].transform.position - transform.position).sqrMagnitude < visionRadius * visionRadius) {
                GameRules.MainPlayer.Ping(index);
                pingCooldown = 1f;
            }
        }
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, visionRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }

    #region Override
    // Runs the action of this organism.
    public override void Action() {
        Human[] humans = (Human[])GameObject.FindObjectsOfType(typeof(Human));
        for (int i = 0; i < humans.Length; i++) {
            if ((humans[i].transform.position - transform.position).sqrMagnitude < attackRadius * attackRadius) {
                Destroy(humans[i].gameObject);
            }
        }
        base.Action();
    }
    #endregion

}
