// Libraries.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Definitions.
using Params = GameRules.Params;

public class Human : Organism {

    // Properties.
    [Space(5), Header("Switches")]
    [SerializeField] public bool initialize;

    [Space(5), Header("Properties")]
    [SerializeField] public Biomass biomass;
    [SerializeField] public Body.BodyData bodyData;
    [SerializeField] public Vision.VisionData visionData;

    [HideInInspector] private Vector3 origin;
    [SerializeField] public float patrolPointA;
    [SerializeField] public float patrolPointB;

    [Space(5), Header("Modules")]
    [SerializeField] public Body body;
    [SerializeField] public Vision vision;

    // Initializes the alien.
    public virtual void Init() {

        body = new Body(transform, bodyData);
        vision = new Vision();

        origin = transform.position;

        initialize = false;
        gameObject.SetActive(true);
    }

    private void Update() {
        if (initialize) {
            Init();
            initialize = false;
        }

        OnUpdate(Time.deltaTime);
    }

    // Runs once per frame.
    float direction = 1f;
    public void OnUpdate(float deltaTime) {

        Think();

        direction = body.velocity.x != 0f ? Mathf.Sign(body.velocity.x) : direction;
        vision.Update(deltaTime, transform.position, 5f, direction, 0.35f);
        body.Update(deltaTime);

        if (health <= 0) {
            OnDeath();
        }
    }

    void Think() {

        if (vision.isScared) {
            body.MoveTo(transform.position);
            body.Jump();
            return;
        }

        Vector3 target = transform.position;
        if (patrolPoints != null && index < patrolPoints.Length) {
            target = patrolPoints[index];
            body.MoveTo(target);
        }
        if ((transform.position - target).sqrMagnitude < GameRules.MovementPrecision * GameRules.MovementPrecision) {
            target = NextPatrolPoint();
            print(target);
            print("hello");
            body.MoveTo(target);
        }
    }

    private Vector3[] patrolPoints = null;
    private int index = 0;
    Vector3 NextPatrolPoint() {
        if (patrolPoints == null) {
            List<Vector3> L_PatrolPoints = new List<Vector3>();
            L_PatrolPoints.Add(origin + Vector3.right * patrolPointA);
            L_PatrolPoints.Add(origin + Vector3.right * patrolPointB);
            patrolPoints = L_PatrolPoints.ToArray();
            index = 0;
            return patrolPoints[0];
        }
        else {
            index = (index + 1) % patrolPoints.Length;
            return patrolPoints[index];
        }
    }

    public void OnHurt(int damage, Vector3 knockback, float magnitude, float duration) {
        health -= damage;
        body.AddKnockback(knockback, magnitude, duration);
    }

    public void OnDeath() {
        // Instantiate(corpse);
        Destroy(gameObject);
    }

    void OnDrawGizmos() {
        direction = body.velocity.x != 0f ? Mathf.Sign(body.velocity.x) : direction;
        Gizmos.DrawWireCube(transform.position + Vector3.right * direction * 2.5f, new Vector3(5f, 0.35f, 1));
        Gizmos.DrawWireCube(transform.position - Vector3.right * direction * 2.5f / 3f, new Vector3(5f / 3f, 0.35f, 1));

        Gizmos.color = Color.red;
        Vector3 position = transform.position;
        if (Application.isPlaying) {
            position = origin;
        }
        Gizmos.DrawWireSphere(position + Vector3.right * patrolPointA, 0.25f);
        Gizmos.DrawWireSphere(position + Vector3.right * patrolPointB, 0.25f);

    }


}
