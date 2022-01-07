using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Incubator {

    [System.Serializable]
    public struct IncubatorData {
        public Egg[] eggs;
        public float maxBiomass;
        public float biomassConversionRate;
        public int maxQueuable;
    }

    [System.Serializable]
    public struct Egg {
        public Alien alien;
        public Sprite eggSprite;
        public float biomassRequired;
    }

    [Space(5), Header("Settings")]
    public bool addToQueue;
    public int index;

    [Space(5), Header("Settings")]
    [HideInInspector] private Queen queen;
    [HideInInspector] public Egg[] eggs;
    [SerializeField, ReadOnly] public float biomass;
    [SerializeField, ReadOnly] public float maxBiomass;
    [SerializeField, ReadOnly] public float biomassConverted;
    [SerializeField, ReadOnly] private float biomassConversionRate;

    [Space(5), Header("Queue")]
    [SerializeField] public List<Egg> queue;
    [SerializeField, ReadOnly] public int maxQueuable;

    public Incubator(Queen queen, IncubatorData incubatorData) {
        this.queen = queen;
        this.eggs = incubatorData.eggs;
        this.maxBiomass = incubatorData.maxBiomass;
        this.biomass = this.maxBiomass;
        this.biomassConversionRate = incubatorData.biomassConversionRate;
        this.maxQueuable = incubatorData.maxQueuable;
        this.queue = new List<Egg>();
    }

    public void Update(float deltaTime) {
        if (addToQueue) {
            AddToQueue(index);
            addToQueue = false;
        }
        ProcessQueue(deltaTime);
    }

    // Adds a new egg to the queue.
    public bool AddToQueue(int index) {

        // Check that this is queuable.
        bool b_CanQueue = index >= 0 && index < eggs.Length && queue.Count < maxQueuable;
        if (!b_CanQueue) {
            return false;
        }

        // Add the egg to the queue.
        queue.Add(eggs[index]);
        return true;
    }

    public bool RemoveFromQueue(int index) {
        
        // Check that this can be removed from the queue.
        bool b_CanRemove = index >= 0 && index < eggs.Length && queue.Count != 0;
        if (!b_CanRemove) {
            return false;
        }

        // Remove this from the queue.
        //
        return false;
    }

    // Processes the queue.
    private void ProcessQueue(float deltaTime) {

        // Check that there is something to process.
        bool b_CanProcess = queue.Count > 0 && biomass >= queue[0].biomassRequired;
        if (!b_CanProcess) {
            return;
        }

        biomassConverted += biomassConversionRate * deltaTime;
        if (biomassConverted >= queue[0].biomassRequired) {
            HatchAlien();
            ResetBiomass();
            queue.RemoveAt(0);
        }

    }

    private void ResetBiomass() {
        biomassConverted = 0f;
    }

    // Spawns the next egg in the queue.
    private void HatchAlien() {
        biomass -= queue[0].biomassRequired;
        Alien alien = UnityEngine.MonoBehaviour.Instantiate(queue[0].alien).GetComponent<Alien>();
        alien.Init(queen);
        queen.nest.AddToNest(alien);
    }
}
