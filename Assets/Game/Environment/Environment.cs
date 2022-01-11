using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Environment : MonoBehaviour {

    public Transform rotator;
    public Wall[] walls;
    public int index;

    private float width = 10f;

    public Vector3 targetRotation;
    public Vector3 eulerAngles;
    public float rotationSpeed;

    private void Start() {

        index = (int)Mathf.Abs(index);
        index = index % walls.Length;
        transform.position = Vector3.forward * width / 2f;

        RefreshMaps();
    }

    private void Update() {

        if (Input.GetKeyDown(KeyCode.P)) {
            Next();
        }
        if (Input.GetKeyDown(KeyCode.O)) {
            Prev();
        }

        Rotate();

    }

    private void Rotate() {
        if (eulerAngles != targetRotation) {

            float rotationDirection = Mathf.Sign(targetRotation.y - eulerAngles.y);

            eulerAngles += Vector3.up * rotationDirection * Time.deltaTime * rotationSpeed;
            if (Mathf.Abs(targetRotation.y - eulerAngles.y) < 2f) {
                eulerAngles = targetRotation;
            }

            rotator.eulerAngles = eulerAngles;
        }
    }

    private void RefreshMaps() {
        for (int i = 0; i < walls.Length; i++) {
            int angleIndex = (index + i) % walls.Length;
            int rightIndex = GetRightIndex(angleIndex);
            int forwardIndex = GetForwardIndex(angleIndex);
            walls[i].transform.eulerAngles = Vector3.up * 90 * angleIndex;
            walls[i].transform.localPosition = Vector3.right * width / 2f * rightIndex;
            walls[i].transform.localPosition += Vector3.forward * width / 2f * forwardIndex;

        }
    }

    private int GetRightIndex(int index) {
        int[] map = new int[4] { 0, 1, 0, -1 };
        return map[index];
    }

    private int GetForwardIndex(int index) {
        int[] map = new int[4] { -1, 0, 1, 0 };
        return map[index];
    }

    void Next() {
        targetRotation -= Vector3.up * 90f;
    }

    void Prev() {
        targetRotation += Vector3.up * 90f;
    }

}
