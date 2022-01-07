// Libraries.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
public class QueenUI : MonoBehaviour {

    // Components.
    [HideInInspector] public Queen queen;
    Canvas canvas;

    // Switches.
    [Space(5), Header("Switches")]
    public bool debugOptions;
    public bool debugQueue;

    // Progress.
    [Space(5), Header("Sliders")]
    public Slider progressSlider;
    public Slider storageSlider;

    // Options.
    [Space(5), Header("Options")]
    public float optionRadius;
    public Transform optionParent;
    [HideInInspector] public List<EggUI> options = new List<EggUI>();

    // Queue.
    [Space(5), Header("Queue")]
    public List<SpriteRenderer> queueDisplay = new List<SpriteRenderer>();
    [Range(0.1f, 0.5f)] public float queueScale;
    public float queueRadius;
    public Transform queueParent;


    // Properties.
    [SerializeField, ReadOnly] private bool isEnabled;

    // Run to initialize.
    public void Init(Queen queen) {
        // Caching.
        canvas = GetComponent<Canvas>();
        this.queen = queen;
        UpdateOptions();
        gameObject.SetActive(true);
    }

    public void OnUpdate() {
        isEnabled = queen.selector.isSelected;
        Display();
    }

    void Display() {
        DisplayOptions();
        DisplayProgress();
        DisplayQueue();
    }

    public void UpdateOptions() {
        for (int i = 0; i < queen.incubator.eggs.Length; i++) {
            EggUI newOption = new GameObject("New Option", typeof(EggUI)).GetComponent<EggUI>();
            newOption.Init(this, i, optionParent);
            options.Add(newOption);
        }
    }

    void DisplayOptions() {
        GameObject optionObject = optionParent.gameObject;
        if (isEnabled) {
            ShowDisplay(optionObject);
        }
        else {
            HideDisplay(optionObject);
        }
    }

    void DisplayQueue() {

        for (int i = 0; i < queueDisplay.Count; i++) {
            Destroy(queueDisplay[i].gameObject);
            queueDisplay[i] = null;
        }
        queueDisplay = new List<SpriteRenderer>();

        for (int i = 0; i < queen.incubator.queue.Count; i++) {
            SpriteRenderer newQueueDisplay = new GameObject("New Queue Display", typeof(SpriteRenderer)).GetComponent<SpriteRenderer>();
            newQueueDisplay.sprite = queen.incubator.queue[i].eggSprite;

            float angle = 360f * (float)i / (float)queen.incubator.maxQueuable;
            newQueueDisplay.transform.position = transform.position + queueRadius * (Quaternion.Euler(0f, 0f, angle) * Vector3.right);
            newQueueDisplay.transform.SetParent(queueParent);
            newQueueDisplay.transform.localScale = new Vector3(queueScale, queueScale, 1f);

            queueDisplay.Add(newQueueDisplay);
        }

        if (queueDisplay.Count > 0) {
            float progress = queen.incubator.biomassConverted / queen.incubator.queue[0].biomassRequired;
            queueDisplay[0].transform.localScale = new Vector3(queueScale + (1f - queueScale) * progress, queueScale + (1f - queueScale) * progress, 1f);
        }

    }

    void DisplayProgress() {

        float storage = queen.incubator.biomass / queen.incubator.maxBiomass;
        storageSlider.value = storage;

        GameObject displayObject = progressSlider.gameObject;

        if (queen.incubator.queue.Count == 0) {
            HideDisplay(displayObject);
            return;
        }

        ShowDisplay(displayObject);
        float progress = queen.incubator.biomassConverted / queen.incubator.queue[0].biomassRequired;
        progressSlider.value = progress;

    }

    void HideDisplay(GameObject gameObject) {
        gameObject.SetActive(false);
    }

    void ShowDisplay(GameObject gameObject) {
        gameObject.SetActive(true);
    }

}
