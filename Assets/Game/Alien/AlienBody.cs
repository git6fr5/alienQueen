// Libraries.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienBody : OrganicBody {

    #region Properties
    [SerializeField] public bool isHidden = false; // Switch that controls whether the body is hidden or not.
    #endregion

    #region Unity
    // Runs once just before every frame.
    private void LateUpdate() {
        Vector3 position = transform.position;
        isHidden = CheckHidden(position);
    }
    #endregion

    #region Override
    // Checks whether this body is using gravity.
    protected override bool CheckGravity() {
        Vector3 position = transform.position;
        bool collisionA = base.CheckSides(position + Vector3.right * GameRules.MovementPrecision, true);
        bool collisionB = base.CheckSides(position - Vector3.right * GameRules.MovementPrecision, false);
        bool collisionC = base.CheckFeet(position + Vector3.up * GameRules.MovementPrecision, true);
        bool collisionD = base.CheckFeet(position + Vector3.up * GameRules.MovementPrecision, false);
        bool isColliding = collisionA || collisionB || collisionC || collisionD;
        return !isColliding;
    }

    // Checks whether this body is hidden.
    private bool CheckHidden(Vector3 position) {

        bool collision = false;
        Vector3 collisionPosition = position + Vector3.right * width / 2f;
        RaycastHit2D[] hits = Physics2D.LinecastAll(collisionPosition - Vector3.down * (height / 2f - GameRules.MovementPrecision),
            collisionPosition + Vector3.down * (height / 2f - GameRules.MovementPrecision));

        collision = CheckHitsForHiding(hits);
        if (collision) { return true; }

        collisionPosition = position + Vector3.left * width / 2f;
        hits = Physics2D.LinecastAll(collisionPosition - Vector3.down * (height / 2f - GameRules.MovementPrecision),
            collisionPosition + Vector3.down * (height / 2f - GameRules.MovementPrecision));

        collision = CheckHitsForHiding(hits);
        if (collision) { return true; }

        return false;

    }
    #endregion

    #region Additional
    private static bool CheckHitsForHiding(RaycastHit2D[] hits) {
        Block block = null;
        for (int i = 0; i < hits.Length; i++) {
            block = hits[i].collider.GetComponent<Block>();
            if (block != null) {
                block.isBeingUsed = true;
                return true;
            }
        }
        return false;
    }
    #endregion

}
