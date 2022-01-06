using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanSpawner : MonoBehaviour {

    public Human human;
    
    void Start() {
        StartCoroutine(IESpawn());
    }

    IEnumerator IESpawn() {
        while (true) {
            GameObject newHuman = Instantiate(human.gameObject);
            newHuman.transform.position = 10f * (Vector3)Random.insideUnitCircle.normalized;
            newHuman.SetActive(true);
            yield return new WaitForSeconds(1f);
        }
    }

}
