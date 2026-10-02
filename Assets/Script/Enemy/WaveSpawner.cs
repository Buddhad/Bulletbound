using System.Collections;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    // =========================================================
    // WAVE SETTINGS
    // =========================================================

    [Header("Wave Settings")]

    // Total time used to spawn all enemies in one wave.
    public float waveDuration = 10f;

    // Number of enemies spawned in each wave.
    public int enemiesPerWave = 5;

    // Total number of waves in the level.
    public int totalWaves = 5;


    // =========================================================
    // ENEMY SETTINGS
    // =========================================================

    [Header("Enemy")]

    // Enemy prefab that will be spawned.
    public GameObject enemyPrefab;

    // Possible positions where enemies can spawn.
    public Transform[] spawnPoints;


    // =========================================================
    // LEVEL COMPLETE
    // =========================================================

    [Header("Level Complete")]

    // UI shown after all waves are completed.
    [SerializeField] private GameObject LevelCompleteScreen;


    // =========================================================
    // INTERNAL VARIABLES
    // =========================================================

    // Current wave number.
    private int currentWave = 0;

    // Prevents multiple wave coroutines
    // from running at the same time.
    private bool wavesStarted = false;


    // =========================================================
    // START
    // =========================================================

    private void Start()
    {
        // GameStartTimer has been removed.
        //
        // Gameplay now starts immediately when
        // the Gameplay scene is loaded.
        //
        // Therefore, start Wave 1 directly.
        StartWave();
    }


    // =========================================================
    // START WAVE
    // =========================================================

    public void StartWave()
    {
        // -----------------------------------------------------
        // CHECK IF ALL WAVES ARE FINISHED
        // -----------------------------------------------------

        if (currentWave >= totalWaves)
        {
            Debug.Log(
                "All waves have already been completed."
            );

            return;
        }


        // -----------------------------------------------------
        // PREVENT DUPLICATE WAVE START
        // -----------------------------------------------------

        // This prevents two wave coroutines from
        // being started at the same time.
        if (wavesStarted)
        {
            return;
        }


        // Mark wave system as active.
        wavesStarted = true;


        // Move to the next wave.
        currentWave++;


        Debug.Log(
            "Starting Wave " +
            currentWave +
            "/" +
            totalWaves
        );


        // Start enemy spawning.
        StartCoroutine(SpawnEnemies());
    }


    // =========================================================
    // SPAWN ENEMIES
    // =========================================================

    private IEnumerator SpawnEnemies()
    {
        Debug.Log(
            "Spawning enemies for Wave " +
            currentWave
        );


        // -----------------------------------------------------
        // CHECK ENEMY PREFAB
        // -----------------------------------------------------

        if (enemyPrefab == null)
        {
            Debug.LogError(
                "WaveSpawner: Enemy Prefab is not assigned!"
            );

            // Allow the system to be started again
            // if the reference is fixed.
            wavesStarted = false;

            yield break;
        }


        // -----------------------------------------------------
        // CHECK SPAWN POINTS
        // -----------------------------------------------------

        if (spawnPoints == null ||
            spawnPoints.Length == 0)
        {
            Debug.LogError(
                "WaveSpawner: No spawn points assigned!"
            );

            wavesStarted = false;

            yield break;
        }


        // -----------------------------------------------------
        // CHECK ENEMY COUNT
        // -----------------------------------------------------

        if (enemiesPerWave <= 0)
        {
            Debug.LogError(
                "WaveSpawner: enemiesPerWave must be greater than 0!"
            );

            wavesStarted = false;

            yield break;
        }


        // =====================================================
        // CALCULATE SPAWN DELAY
        // =====================================================

        // Example:
        //
        // Wave Duration = 10 seconds
        // Enemies = 5
        //
        // 10 / 5 = 2 seconds
        //
        // Therefore, one enemy spawns every 2 seconds.

        float spawnDelay =
            waveDuration / enemiesPerWave;


        // =====================================================
        // SPAWN ENEMIES
        // =====================================================

        for (int i = 0; i < enemiesPerWave; i++)
        {
            // Select a random spawn point.
            int randomIndex =
                Random.Range(
                    0,
                    spawnPoints.Length
                );


            // -------------------------------------------------
            // CHECK SPAWN POINT
            // -------------------------------------------------

            if (spawnPoints[randomIndex] == null)
            {
                Debug.LogWarning(
                    "WaveSpawner: Spawn Point " +
                    randomIndex +
                    " is not assigned."
                );


                // Wait before attempting the next spawn.
                yield return new WaitForSeconds(
                    spawnDelay
                );

                continue;
            }


            // -------------------------------------------------
            // CREATE ENEMY
            // -------------------------------------------------

            Instantiate(
                enemyPrefab,
                spawnPoints[randomIndex].position,
                Quaternion.identity
            );


            Debug.Log(
                "Enemy " +
                (i + 1) +
                "/" +
                enemiesPerWave +
                " spawned."
            );


            // Wait before spawning the next enemy.
            yield return new WaitForSeconds(
                spawnDelay
            );
        }


        // =====================================================
        // WAIT FOR ALL ENEMIES TO DIE
        // =====================================================

        Debug.Log(
            "Wave " +
            currentWave +
            " spawning completed."
        );


        Debug.Log(
            "Waiting for all enemies from Wave " +
            currentWave +
            " to be defeated..."
        );


        // Continue waiting while at least one Enemy
        // still exists in the scene.
        while (
            GameObject.FindGameObjectsWithTag("Enemy").Length > 0
        )
        {
            yield return null;
        }


        // =====================================================
        // CURRENT WAVE COMPLETED
        // =====================================================

        Debug.Log(
            "Wave " +
            currentWave +
            " completed."
        );


        // =====================================================
        // START NEXT WAVE
        // =====================================================

        if (currentWave < totalWaves)
        {
            // Allow StartWave() to start another wave.
            wavesStarted = false;


            // Start the next wave.
            StartWave();
        }
        else
        {
            // All waves are completed.
            yield return StartCoroutine(
                CompleteLevel()
            );
        }
    }


    // =========================================================
    // LEVEL COMPLETE
    // =========================================================

    private IEnumerator CompleteLevel()
    {
        Debug.Log(
            "All " +
            totalWaves +
            " waves completed!"
        );


        // =====================================================
        // WAIT FOR REMAINING COINS
        // =====================================================

        // Enemies now ONLY drop coins.
        //
        // Abilities are no longer dropped by enemies,
        // so we do NOT need to check the Ability layer.
        Debug.Log(
            "Waiting for remaining coins..."
        );


        while (true)
        {
            // Check whether any Coin remains in the scene.
            bool coinsExist =
                GameObject.FindGameObjectsWithTag(
                    "Coin"
                ).Length > 0;


            // If there are no coins left,
            // continue to level completion.
            if (!coinsExist)
            {
                break;
            }


            // Check again next frame.
            yield return null;
        }


        Debug.Log(
            "No remaining coins."
        );


        // =====================================================
        // STOP PLAYER
        // =====================================================

        // Find the Player.
        GameObject player =
            GameObject.FindWithTag("Player");


        if (player != null)
        {
            // Get PlayerMovement.
            PlayerMovement movement =
                player.GetComponent<PlayerMovement>();


            if (movement != null)
            {
                // Stop the Player's horizontal movement.
                movement.ForceIdle();


                // Disable Player movement.
                movement.enabled = false;
            }
        }
        else
        {
            Debug.LogWarning(
                "WaveSpawner: Player not found."
            );
        }


        // =====================================================
        // SMALL DELAY
        // =====================================================

        // Give the game a short delay before
        // showing the Level Complete screen.
        yield return new WaitForSeconds(2f);


        // =====================================================
        // UPDATE HIGH SCORE
        // =====================================================

        // Get previously saved High Score.
        int previousHighScore =
            PlayerPrefs.GetInt(
                "HighScore",
                0
            );


        // Check whether the current score
        // is higher than the saved score.
        if (ScoreManager.CurrentScore >
            previousHighScore)
        {
            // Save the new High Score.
            PlayerPrefs.SetInt(
                "HighScore",
                ScoreManager.CurrentScore
            );


            // Save PlayerPrefs immediately.
            PlayerPrefs.Save();


            Debug.Log(
                "New High Score: " +
                ScoreManager.CurrentScore
            );
        }


        // =====================================================
        // SHOW LEVEL COMPLETE SCREEN
        // =====================================================

        if (LevelCompleteScreen != null)
        {
            // Enable the Level Complete UI.
            LevelCompleteScreen.SetActive(true);


            Debug.Log(
                "Level Complete Screen displayed."
            );
        }
        else
        {
            Debug.LogWarning(
                "WaveSpawner: LevelCompleteScreen is not assigned."
            );
        }
    }
}