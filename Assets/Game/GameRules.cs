using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadOnlyAttribute : PropertyAttribute {

}

public class GameRules : MonoBehaviour {

    [System.Serializable]
    public struct Params {

        public float mid => (min + max) / 2f;
        public float range => max - min;

        public float fValue => Random.Range(min, max);
        public int iValue => (int)Mathf.Round(Random.Range(min, max));

        public float min;
        public float max;

        public Params(float min, float max) {
            this.min = min;
            this.max = max;
        }

        public float Evaluate(float ratio) {
            ratio = ratio > 1 ? 1 : (ratio < 0 ? 0 : ratio);
            return min + (max - min) * ratio;
        }

        public float Ratio(float value) {
            value = value < min ? min : (value > max ? max : value);
            return (value - min) / max;
        }

        public float Ratio(int value) {
            return Ratio((float)value);
        }

        public float Clamp(float value) {
            return value = value > max ? max : (value < min ? min : value);
        }

    }

    public static float OutlineWidth = 1f / 16f;

    public static UnityEngine.Camera MainCamera;
    public static Vector3 MousePosition => MainCamera.ScreenToWorldPoint(UnityEngine.Input.mousePosition);

    void Start() {
        MainCamera = Camera.main;
    }

    public Vector3 cachedPositionA;
    public Vector3 cachedPositionB;

    void Update() {


        if (Input.GetMouseButtonDown(0)) {
            cachedPositionA = MousePosition;
        }
        if (Input.GetMouseButton(0)) {
            cachedPositionB = MousePosition;
        }

        if (Input.GetMouseButtonUp(0)) {
            // Select
            Select();

            // Reset
            cachedPositionA = Vector3.zero;
            cachedPositionB = Vector3.zero;
        }

        if (Input.GetKeyDown(KeyCode.S)) {
            Seperate(1f);
        }

        if (Input.GetKey(KeyCode.S)) {
            float deltaTime = Time.deltaTime;
            Seperate(deltaTime);
        }

    }

    void Seperate(float force) {

        Alien[] aliens = (Alien[])GameObject.FindObjectsOfType(typeof(Alien));
        List<Alien> selectedAliens = new List<Alien>();

        for (int i = 0; i < aliens.Length; i++) {
            if (aliens[i].isSelected) {
                selectedAliens.Add(aliens[i]);
            }
        }

        for (int i = 0; i < selectedAliens.Count; i++) {

            Vector3 targetDirection = Quaternion.Euler(0f, 0f, 360f * (float)i / (float)selectedAliens.Count) * Vector3.right;
            selectedAliens[i].target.parent = null;
            selectedAliens[i].target.position += targetDirection * force;

        }

    }

    void Select() {

        Collider2D[] hits = Physics2D.OverlapAreaAll(cachedPositionA, cachedPositionB);
        for (int i = 0; i < hits.Length; i++) {
            Alien alien = hits[i].GetComponent<Alien>();
            if (alien != null) {
                alien.isSelected = true;
            }
        }

    }

    void OnDrawGizmos() {

        Vector3 center = (cachedPositionA + cachedPositionB) / 2f;
        float width = Mathf.Abs(cachedPositionA.x - cachedPositionB.x);
        float height = Mathf.Abs(cachedPositionA.y - cachedPositionB.y);
        center.z = 0f;

        Gizmos.DrawWireCube(center, new Vector3(width, height, 0.1f));

    }

}
