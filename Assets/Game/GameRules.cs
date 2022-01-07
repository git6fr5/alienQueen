using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public static float MovementPrecision = 0.05f;

    public static float OutlineWidth = 1f / 16f;

    public static UnityEngine.Camera MainCamera;
    public static Vector3 MousePosition => MainCamera.ScreenToWorldPoint(UnityEngine.Input.mousePosition);

    public Sprite biomassSprite;
    public static Sprite BiomassSprite;

    public float gravity;
    public static float Gravity;

    public bool editing;

    void Start() {
        MainCamera = Camera.main;
        BiomassSprite = biomassSprite;
        Gravity = gravity;
    }

    void Update() {
        if (editing) {
            Gravity = gravity;
        }
    }

    void OnDrawGizmos() {


    }

}

public class ReadOnlyAttribute : PropertyAttribute {

}
