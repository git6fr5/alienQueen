using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRules : MonoBehaviour {

    /* --- Static Tags --- */
    public static string PlayerTag = "Player";
    public static string GroundTag = "Ground";
    
    /* --- Static Variables --- */
    // Player.
    public static Controller MainPlayer;
    // Objects.
    public static GameObject BackgroundObject;
    public static GameObject GameOverObject;
    public static GameObject GlobalLightObject;
    // Movement.
    public static float VelocityDamping = 0.95f;
    public static float MovementPrecision = 0.05f;
    public static float GravityScale = -50f;
    // Animation.
    public static float FrameRate = 24f;
    public static float OutlineWidth = 1f / 16f;
    // Camera.
    public static UnityEngine.Camera MainCamera;
    public static Vector3 MousePosition => MainCamera.ScreenToWorldPoint(UnityEngine.Input.mousePosition);

    /* --- Properties --- */
    public Controller mainPlayer;
    public GameObject backgroundObject;
    public GameObject globalLightObject;
    public GameObject gameOverObject;
    public float velocityDamping = 0.95f;
    public float gravityScale;
    public float frameRate;
    public int cameraX;
    public int cameraY;
    public int pixelsPerUnit;
    public bool reset;
    public bool followPlayer;
    public Vector3 followOffset;

    /* --- Unity --- */
    // Runs once before the first frame.
    void Start() {
        Init();
    }

    // Runs once every frame.
    private void Update() {
        if (followPlayer) {
            MainCamera.transform.position = MainPlayer.transform.position + followOffset;
        }
    }

    /* --- Methods --- */
    private void Init() {
        // Set these static variables.
        MainCamera = Camera.main;
        MainPlayer = mainPlayer;

        BackgroundObject = backgroundObject;
        GameOverObject = gameOverObject;
        GlobalLightObject = globalLightObject;
        // GlobalLightObject.SetActive(false);

        VelocityDamping = velocityDamping;
        GravityScale = gravityScale;
        FrameRate = frameRate;
    }

    /* --- Events --- */
    public static void GameOver() {
        GameOverObject.SetActive(true);
        Time.timeScale = 0f;
    }

    /* --- Debug --- */
    void OnDrawGizmos() {
        Gizmos.DrawWireCube(transform.position, new Vector3(cameraX, cameraY, 1f));
    }

}

public class ReadOnlyAttribute : PropertyAttribute {

}
