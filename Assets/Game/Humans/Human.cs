// Libraries.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Organism))]
public class Human : MonoBehaviour {

    #region Components
    [HideInInspector] private Organism organism;
    [HideInInspector] private Player player;
    #endregion

    #region Parameters
    [Space(2), Header("Patrol")]
    [SerializeField] public Transform[] patrolPoints;
    [SerializeField, ReadOnly] private int patrolIndex;
    [Space(2), Header("Vision")]
    [SerializeField] private float visionAngle;
    [SerializeField, ReadOnly] private float correctedVisionAngle;
    [SerializeField] private float visionDistance;
    #endregion

    // temp.
    [SerializeField] private Transform visionTransform;
    [HideInInspector] private Vector3 visionTransformScale;

    #region Unity
    // Runs once before the first frame.
    private void Start() {
        Init();
    }

    // Runs once every frame.
    private void Update() {
        // Cache the time differential.
        float deltaTime = Time.deltaTime;
        ProcessLogic();
        Vision();
    }

    // Runs once every draw call.
    void OnDrawGizmos() {
        Gizmos.color = Color.red;
        if (patrolPoints != null) {
            for (int i = 0; i < patrolPoints.Length; i++) {
                Gizmos.DrawWireSphere(patrolPoints[i].position, 0.1f);
            }
        }

        // float currVisionAngle = visionAngle;
        //if (organism != null && !organism.organicBody.facingRight) {
        //    currVisionAngle += 180f;
        //}

        Gizmos.color = new Color(1f, 1f, 1f, 0.5f);

        Vector3 axis = Vector3.right;
        if (organism != null) {
            axis = organism.organicBody.facingRight ? Vector3.right : Vector3.left;
        }

        Gizmos.DrawLine(transform.position, transform.position + Quaternion.Euler(0f, 0f, visionAngle) * axis * visionDistance);
        Gizmos.DrawLine(transform.position, transform.position + Quaternion.Euler(0f, 0f, -visionAngle) * axis * visionDistance);
        
        Gizmos.color = new Color(1f, 1f, 1f, 0.25f);
        Gizmos.DrawWireSphere(transform.position, visionDistance);

    }
    #endregion

    #region Methods
    private void Init() {
        organism = GetComponent<Organism>();
        patrolIndex = 0;
        for (int i = 0; i < patrolPoints.Length; i++) {
            patrolPoints[i].transform.SetParent(null);
        }
        visionTransformScale = visionTransform.localScale;
    }

    private float pauseTimer = 0f;

    private void ProcessLogic() {

        OrganicBody organicBody = organism.organicBody;

        pauseTimer -= Time.deltaTime;
        if (pauseTimer > 0f) {
            organicBody.MoveTo(transform.position);
            return;
        }
        pauseTimer = 0f;

        Vector3 target = transform.position;
        target.x = patrolPoints[patrolIndex].transform.position.x;
        if (Mathf.Abs(organicBody.transform.position.x - target.x) < GameRules.MovementPrecision){
            pauseTimer = 1f;
            patrolIndex = (patrolIndex + 1) % patrolPoints.Length;
        }
        organicBody.MoveTo(target);
    }

    private void Vision() {

        visionTransform.localScale = new Vector3(organism.organicBody.facingRight ? visionTransformScale.x : -visionTransformScale.x, visionTransformScale.y, visionTransformScale.z);

        for (int i = 0; i < GameRules.MainPlayer.organisms.Length; i++) {
            Alien alien = GameRules.MainPlayer.organisms[i].GetComponent<Alien>();
            if (alien != null) {
                CheckAlien(alien);
            }

        }
    
    }

    private void CheckAlien(Alien alien) {

        AlienBody alienBody = alien.GetComponent<AlienBody>();
        if (alienBody!= null && alienBody.isHidden) {
            return;
        }

        Vector3 alienPos = alien.transform.position;
        Vector3 alienDisp = alienPos - transform.position;

        Vector3 axis = organism.organicBody.facingRight ? Vector3.right : Vector3.left;
        float angleToAlien = Vector2.SignedAngle(axis, (Vector2)alienDisp);
        float distanceToAlien = alienDisp.magnitude;

        bool withinAngle = angleToAlien < visionAngle && angleToAlien > -visionAngle;
        bool withinDistance = distanceToAlien < visionDistance;

        if (withinDistance && withinAngle) {
            GameRules.KillAlien(alien);
            // GameRules.GameOver();
        }
    }
    #endregion

}
