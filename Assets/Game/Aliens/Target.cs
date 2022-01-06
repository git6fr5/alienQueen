using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour {

    void FixedUpdate() {

        //Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 0.05f);
        //for (int i = 0; i < hits.Length; i++) {

        //    Human human = hits[i].GetComponent<Human>();
        //    if (human != null) {
        //        transform.parent = human.transform;
        //        transform.localPosition = Vector3.zero;
        //    }

        //}

    }

    void OnDrawGizmos() {
        Gizmos.DrawWireSphere(transform.position, 0.05f);
    }

}
