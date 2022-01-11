using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class AlienAnimator : MonoBehaviour {

    // Components.
    SpriteRenderer spriteRenderer;
    Alien alien;

    [Space(5), Header("Internal Controls")]
    [SerializeField, ReadOnly] private Sprite[] anim;
    [HideInInspector] private Sprite[] prevAnim;
    [SerializeField, ReadOnly] private int index;
    [SerializeField, ReadOnly] private bool flip;

    [Space(5), Header("Settings")]
    [SerializeField] public Sprite[] idle;
    [SerializeField] public Sprite[] walk;

    void Start() {
        Init();
    }

    void Init() {
        alien = transform.parent.GetComponent<Alien>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(IEAnimate());
    }

    void Update() {
        Render();
    }

    void Render() {

        float f_OutlineWidth = alien.selector.isSelected ? GameRules.OutlineWidth : 0f;
        spriteRenderer.material.SetFloat("_OutlineWidth", f_OutlineWidth);

        float f_Angle = flip ? 180f : 0f;
        transform.eulerAngles = Vector3.up * f_Angle;
    }

    private IEnumerator IEAnimate() {
        while (true) {
            bool b_NewAnimation = DecideAnimation();
            FrameUpdate(false);
            yield return new WaitForSeconds(1f / GameRules.FrameRate);
        }
    }

    bool DecideAnimation() {

        if (alien.body.acceleration.x != 0f) {
            anim = walk;
            flip = alien.body.acceleration.x < 0f ? true : false;
        }
        else {
            anim = idle;
        }

        bool b_NewAnimation = false;
        if (anim != prevAnim) {
            index = 0;
            prevAnim = anim;
            b_NewAnimation = true;
        }

        return b_NewAnimation;

    }

    void FrameUpdate(bool newAnimation) {
        if (!newAnimation) {
            index = (index + 1) % anim.Length;
        }
        spriteRenderer.sprite = anim[index];
    }

}
