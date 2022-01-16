using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRules : MonoBehaviour {

    #region Static Variables
    // Player.
    public static Player MainPlayer;
    public static GameObject GameOverObject;
    // Movement.
    public static float VelocityDamping = 0.95f;
    public static float MovementPrecision = 0.05f;
    public static float Gravity = -50f;
    // Animation.
    public static float FrameRate = 24f;
    public static float OutlineWidth = 1f / 16f;
    public static Sprite BiomassSprite;
    // Camera.
    public static UnityEngine.Camera MainCamera;
    public static Vector3 MousePosition => MainCamera.ScreenToWorldPoint(UnityEngine.Input.mousePosition);
    #endregion

    #region Properties
    public Player mainPlayer;
    public GameObject globalLight;
    public GameObject gameOverObject;
    public float velocityDamping = 0.95f;
    public float gravity;
    public float frameRate;
    public Sprite biomassSprite;
    public bool reset;
    public int cameraX;
    public int cameraY;
    public int pixelsPerUnit;
    #endregion

    void Start() {
        MainCamera = Camera.main;
        GameOverObject = gameOverObject;
        BiomassSprite = biomassSprite;
        MainPlayer = mainPlayer;
        globalLight.SetActive(false);
        StartCoroutine(IEReset());
    }

    private IEnumerator IEReset() {
        while (true) {
            yield return new WaitForSeconds(0.2f);
            VelocityDamping = velocityDamping;
            Gravity = gravity;
            frameRate = FrameRate;
        }
    }

    public static void KillAlien(Alien alien) {

        List<Organism> temp = new List<Organism>();
        for (int i = 0; i < MainPlayer.organisms.Length; i++) {
            temp.Add(MainPlayer.organisms[i]);
        }
        temp.Remove(alien);
        MainPlayer.organisms = temp.ToArray();
        MainPlayer.organismIndex = 0;

        Destroy(alien.gameObject);

        if (temp == null || temp.Count == 0) {
            GameOver();
        }

    }

    public static void GameOver() {
        GameOverObject.SetActive(true);
        Time.timeScale = 0f;
    }

    void OnDrawGizmos() {
        Gizmos.DrawWireCube(transform.position, new Vector3(cameraX, cameraY, 1f));
    }

}

public class ReadOnlyAttribute : PropertyAttribute {

}
