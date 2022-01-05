// Libraries.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(BoxCollider2D))]
public class QueenUI : MonoBehaviour {

    // Components.
    [HideInInspector] public Queen queen;
    Canvas canvas;

    // Switches.
    [Space(5), Header("Switches")]
    public bool debugOptions;
    public bool debugQueue;

    // Properties.
    public bool isSelected;
    public bool isMouseOver;

    // Progress.
    [Space(5), Header("Progress")]
    public Slider progressSlider;

    // Options.
    [Space(5), Header("Options")]
    public float optionRadius;
    public Transform optionParent;
    private List<EggUI> options = new List<EggUI>();

    // Queue.
    [Space(5), Header("Queue")]
    public List<SpriteRenderer> queueDisplay = new List<SpriteRenderer>();
    [Range(0.1f, 0.5f)] public float queueScale;
    public float queueRadius;
    public Transform queueParent;

    // Runs once before the first frame.
    void Start() {
        // Caching.
        queen = transform.parent.GetComponent<Queen>();
        canvas = GetComponent<Canvas>();

        // Set up.
        // canvas.eventCamera = Camera.main;
        CreateOptions();
    }

    // Runs once every frame.
    void Update() {
        CheckSelect();
        DisplayOptions();
        DisplayProgress();
        DisplayQueue();
    }

    void OnMouseOver() {
        isMouseOver = true;
    }

    void OnMouseExit() {
        isMouseOver = false;
    }


    void CheckSelect() {

        if (Input.GetMouseButtonDown(0) && !isSelected) {
            isSelected = isMouseOver;
        }
        if (Input.GetMouseButtonDown(1) && isSelected) {
            isSelected = false;
        }
    }

    void CreateOptions() {
        for (int i = 0; i < queen.eggs.Length; i++) {
            EggUI newOption = new GameObject("New Option", typeof(EggUI)).GetComponent<EggUI>();
            newOption.Init(this, i, optionParent);
            options.Add(newOption);
        }
    }

    void DisplayOptions() {
        GameObject optionObject = optionParent.gameObject;
        if (isSelected) {
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

        for (int i = 0; i < queen.queue.Count; i++) {
            SpriteRenderer newQueueDisplay = new GameObject("New Queue Display", typeof(SpriteRenderer)).GetComponent<SpriteRenderer>();
            newQueueDisplay.sprite = queen.queue[i].eggSprite;

            float angle = 360f * (float)i / (float)queen.maxQueuable;
            newQueueDisplay.transform.position = transform.position + queueRadius * (Quaternion.Euler(0f, 0f, angle) * Vector3.right);
            newQueueDisplay.transform.SetParent(queueParent);
            newQueueDisplay.transform.localScale = new Vector3(queueScale, queueScale, 1f);
            
            queueDisplay.Add(newQueueDisplay);
        }

        if (queueDisplay.Count > 0) {
            float progress = queen.biomassConverted / queen.queue[0].biomass;
            queueDisplay[0].transform.localScale = new Vector3(queueScale + (1f - queueScale) * progress, queueScale + (1f - queueScale) * progress, 1f);
        }

    }

    void DisplayProgress() {

        GameObject displayObject = progressSlider.gameObject;

        if (queen.queue.Count == 0) {
            HideDisplay(displayObject);
            return;
        }

        ShowDisplay(displayObject);
        float progress = queen.biomassConverted / queen.queue[0].biomass;
        progressSlider.value = progress;

    }

    void HideDisplay(GameObject gameObject) {
        gameObject.SetActive(false);
    }

    void ShowDisplay(GameObject gameObject) {
        gameObject.SetActive(true);
    }

    void OnDrawGizmos() {

        if (debugOptions) {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, optionRadius);
        }

        if (debugQueue) {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, queueRadius);
        }

    }

}
