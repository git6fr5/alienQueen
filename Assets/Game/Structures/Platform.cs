/* --- Libraries --- */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
public class Platform : MonoBehaviour {

    /* --- Parameters --- */
    [SerializeField] protected Vector3 target = Vector3.zero;
    [SerializeField] private float speed = 0f;

    /* --- Properties --- */
    [SerializeField, ReadOnly] protected List<Controller> container = new List<Controller>();

    /* --- Unity --- */
    // Runs once before the first frame.
    private void Start() {
        Init();
    }

    // Runs once every frame.
    private void Update() {
        Target();
    }

    // Runs once every frame.
    private void FixedUpdate() {
        float deltaTime = Time.fixedDeltaTime;
        Move(deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        Controller controller = collision.gameObject.GetComponent<Controller>();
        if (controller != null && !container.Contains(controller)) {
            container.Add(controller);
        }
    }

    private void OnCollisionExit2D(Collision2D collision) {
        Controller controller = collision.gameObject.GetComponent<Controller>();
        if (controller != null && container.Contains(controller)) {
            container.Remove(controller);
        }
    }

    /* --- Virtual Methods --- */
    // Runs the initialization logic.
    protected virtual void Init() {
        target = transform.position;
    }

    // Sets the target for this platform.
    protected virtual void Target() {
        // 
    }

    /* --- Methods --- */
    // Moves this platform.
    private void Move(float deltaTime) {
        Vector3 velocity = (target - transform.position).normalized * speed;
        transform.position += velocity * deltaTime;
        for (int i = 0; i < container.Count; i++) {
            container[i].transform.position += velocity * deltaTime;
        }
    }

}
