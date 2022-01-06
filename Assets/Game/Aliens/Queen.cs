using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Queen : MonoBehaviour {

    [System.Serializable]
    public struct Egg {

        [HideInInspector] public int index;
        public Alien alien;
        public Sprite eggSprite;
        public float biomass; 

    }

    [Space(5), Header("Switches")]
    public bool debugHatching;

    // Properties.
    public bool isSelected;
    public bool isMouseOver;

    // Properties.
    [Space(5), Header("Alien Eggs")]
    public Egg[] eggs;
    [HideInInspector] public List<Alien> nest = new List<Alien>();

    [Space(5), Header("Settings")]
    public float maxBiomass;
    [HideInInspector] public float biomass;
    public float biomassConverted;
    public float biomassConversionRate;
    public float hatchRadius;

    [Space(5), Header("Queue")]
    public List<Egg> queue;
    public int maxQueuable;
    public bool addToQueue;
    public int index;

    // Runs once before the first frame.
    void Start() {
        biomass = maxBiomass;
        RefreshEggs();
    }

    void RefreshEggs() {
        for (int i = 0; i < eggs.Length; i++) {
            eggs[i].index = i;
        }
    }

    // Runs once every frame.
    void Update() {

        float deltaTime = Time.deltaTime;

        if (addToQueue) {
            AddToQueue(index);
            addToQueue = false;
        }

        ProcessQueue(deltaTime);
    }

    // Adds a new egg to the queue.
    public bool AddToQueue(int index) {

        if (index < 0 || index >= eggs.Length) {
            return false;
        }

        if (queue.Count >= maxQueuable) {
            return false;
        }

        queue.Add(eggs[index]);
        return true;
    }

    public bool RemoveFromQueue(int index) {

        if (index < 0 || index >= eggs.Length) {
            return false;
        }

        if (queue.Count == 0) {
            return false;
        }

        for (int i = queue.Count - 1; i >= 0; i--) {
            int getIndex = -1;
            for (int j = 0; j < eggs.Length; j++) {
                if (queue[i].index == eggs[j].index) {
                    getIndex = j;
                    break;
                }
            }

            if (getIndex == index) {
                queue.RemoveAt(i);
                if (i == 0) {
                    ResetBiomass();
                }
                return true;
            }
        }
        return false;
    }

    // Processes the queue.
    private void ProcessQueue(float deltaTime) {

        if (queue.Count == 0) {
            return;
        }

        if (biomass < queue[0].biomass) {
            return;
        }

        biomassConverted += biomassConversionRate * deltaTime;
        if (biomassConverted >= queue[0].biomass) {
            HatchAlien();
            ResetBiomass();
            queue.RemoveAt(0);
        }

    }

    // Spawns the next egg in the queue.
    private void HatchAlien() {
        biomass -= queue[0].biomass;
        Alien alien = Instantiate(queue[0].alien).GetComponent<Alien>();
        alien.Init(this);
        nest.Add(alien);
    }

    private void ResetBiomass() {
        biomassConverted = 0f;
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

    void OnMouseOver() {
        isMouseOver = true;
    }

    void OnMouseExit() {
        isMouseOver = false;
    }


    void OnDrawGizmos() {

        if (debugHatching) {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, hatchRadius);
        }

    }

}
