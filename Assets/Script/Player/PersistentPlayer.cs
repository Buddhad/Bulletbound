using UnityEngine;
using UnityEngine.SceneManagement;

public class PersistentPlayer : MonoBehaviour
{
    // =========================================================
    // SINGLE PLAYER INSTANCE
    // =========================================================

    public static PersistentPlayer Instance;


    // =========================================================
    // AWAKE
    // =========================================================

    private void Awake()
    {
        // -----------------------------------------------------
        // PREVENT DUPLICATE PLAYERS
        // -----------------------------------------------------

        // If another persistent Player already exists,
        // destroy this duplicate.
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }


        // This becomes the main persistent Player.
        Instance = this;


        // Keep this Player alive when changing scenes.
        DontDestroyOnLoad(gameObject);
    }


    // =========================================================
    // ENABLE
    // =========================================================

    private void OnEnable()
    {
        // Listen for scene changes.
        SceneManager.sceneLoaded += OnSceneLoaded;
    }


    // =========================================================
    // DISABLE
    // =========================================================

    private void OnDisable()
    {
        // Stop listening for scene changes.
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }


    // =========================================================
    // NEW SCENE LOADED
    // =========================================================

    private void OnSceneLoaded(
        Scene scene,
        LoadSceneMode mode)
    {
        Debug.Log(
            "Persistent Player entered scene: " +
            scene.name
        );


        // -----------------------------------------------------
        // FIND PLAYER SPAWN POINT
        // -----------------------------------------------------

        // Each level should contain ONE object tagged:
        // PlayerSpawn
        GameObject spawnPoint =
            GameObject.FindGameObjectWithTag(
                "PlayerSpawn"
            );


        if (spawnPoint != null)
        {
            // Move the existing Player to the
            // new level's spawn position.
            transform.position =
                spawnPoint.transform.position;


            Debug.Log(
                "Player moved to spawn point in: " +
                scene.name
            );
        }
        else
        {
            Debug.LogWarning(
                "PersistentPlayer: PlayerSpawn point " +
                "not found in scene: " +
                scene.name
            );
        }


        // -----------------------------------------------------
        // RESET PLAYER PHYSICS
        // -----------------------------------------------------

        Rigidbody2D rb =
            GetComponent<Rigidbody2D>();


        if (rb != null)
        {
            // Remove any movement left from the
            // previous level.
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }


        // -----------------------------------------------------
        // RE-ENABLE PLAYER MOVEMENT
        // -----------------------------------------------------

        PlayerMovement movement =
            GetComponent<PlayerMovement>();


        if (movement != null)
        {
            // WaveSpawner disables movement when a level
            // is completed.
            //
            // Enable it again for the new level.
            movement.enabled = true;

            movement.SetMovementEnabled(true);

            // Put the Player into idle animation/state.
            movement.ForceIdle();
        }
        else
        {
            Debug.LogWarning(
                "PersistentPlayer: PlayerMovement not found."
            );
        }
    }
}