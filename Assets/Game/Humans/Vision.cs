using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Vision {

    public bool isScared;
    public float scaredDuration;
    public float scaredTicks = 0f;

    [System.Serializable]
    public struct VisionData {
        
    }

    public Vision() {
        scaredDuration = 0.5f;
    }

    public void Update(float deltaTime, Vector3 position, float size, float direction, float height) {
        //
        if (isScared) {
            scaredTicks += deltaTime;
            if (scaredTicks > scaredDuration) {
                isScared = false;
                scaredTicks = 0f;
            }
            return;
        }

        Vector3 pointA = position + Vector3.right * direction * size + Vector3.up * height / 2f;
        Vector3 pointB = position - Vector3.right * direction * size / 3f - Vector3.up * height / 2f;
        Collider2D[] hits = Physics2D.OverlapAreaAll(pointA, pointB);

        for (int i = 0; i < hits.Length; i++) {
            Alien alien = hits[i].GetComponent<Alien>();
            if (alien != null) {
                Debug.Log("Scared");
                isScared = true;
            }
        }

    }

}
