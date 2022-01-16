// Libraries.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {

    #region Components
    [SerializeField] public Organism[] organisms;
    #endregion

    #region Parameters
    [SerializeField, ReadOnly] public int organismIndex;
    [SerializeField, ReadOnly] private KeyCode swapKey = KeyCode.Tab;
    [SerializeField, ReadOnly] private KeyCode jumpKey = KeyCode.Space;
    [SerializeField, ReadOnly] private KeyCode actionKey = KeyCode.J;
    #endregion

    // temp
    public bool follow;
    public Vector3 followOffset;

    #region Unity
    // Runs once before the first frame.
    private void Start() {
        Init();
    }

    // Runs once every frame.
    private void Update() {
        // Cache the time differential.
        float deltaTime = Time.deltaTime;
        ProcessInput();
        ProcessCamera();
    }
    #endregion

    #region Methods
    private void Init() {
        if (organisms == null || organisms.Length == 0) {
            print("Error: Supply some organisms for the player to control");
        }
        organismIndex = 0;
        for (int i = 0; i < organisms.Length; i++) {
            Alien alien = organisms[i].GetComponent<Alien>();
            if (alien != null) {
                alien.index = i;
            }

            CreateAlienUI(i);
        }
    }

    private void ProcessInput() {
        if (Input.GetKeyDown(swapKey)) {
            organismIndex = (organismIndex + 1) % organisms.Length;
        }

        for (int i = 1; i < 9; i++) {
            if (Input.GetKeyDown(i.ToString()) && (i-1) < organisms.Length) {
                organismIndex = (i-1);
            }

        }

        Organism organism = organisms[organismIndex];
        OrganicBody organicBody = organism.organicBody;
        Vector3 target = organicBody.transform.position + new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0f);
        organicBody.MoveTo(target);
        if (Input.GetKeyDown(jumpKey)) {
            organicBody.Jump();
        }
        if (Input.GetKeyDown(actionKey)) {
            organism.Action();
        }
    }

    private void ProcessCamera() {
        if (follow) {
            GameRules.MainCamera.transform.position = organisms[organismIndex].transform.position + followOffset;
        }

        RectTransform rt = alienImage.GetComponent<RectTransform>();
        for (int i = 0; i < organisms.Length; i++) {
            alienUI[i].GetComponent<RectTransform>().localScale = rt.localScale;
            // alienUI[i].color = Color.white;
        }
        alienUI[organismIndex].GetComponent<RectTransform>().localScale = rt.localScale * 1.25f;
        // alienUI[organismIndex].color = new Color(1f, 1f, 0f, 1f);
    }
    #endregion

    // temp
    public void Ping(int index) {
        GetComponent<AudioSource>().Play();
        StartCoroutine(IEPinging(index, 10));
    }

    private IEnumerator IEPinging(int index, float n = 10) {
        Color originalColor = alienUI[index].color;
        for (int i = 0; i < n; i++) {
            if (i % 2 == 0) {
                alienUI[index].color = Color.red;
            }
            else {
                alienUI[index].color = originalColor;
            }
            print("pinging");
            yield return new WaitForSeconds(1f / n);
        }
        alienUI[index].color = originalColor;
        yield return null;
    }

    // temp.
    [SerializeField, ReadOnly] public List<Image> alienUI = new List<Image>();
    public Image alienImage;
    private void CreateAlienUI(int index) {
        RectTransform rt = alienImage.GetComponent<RectTransform>();
        float width = rt.sizeDelta.x;
        float height = rt.sizeDelta.y;

        GameObject newObject = Instantiate(alienImage.gameObject);
        Image newImage = newObject.GetComponent<Image>();
        newImage.transform.SetParent(alienImage.transform.parent);

        newImage.GetComponent<RectTransform>().localPosition = new Vector3(rt.localPosition.x + index * (width + 5f), rt.localPosition.y, rt.localPosition.z);
        newImage.GetComponent<RectTransform>().localScale = rt.localScale;

        newObject.SetActive(true);

        alienUI.Add(newImage);
    }

}
