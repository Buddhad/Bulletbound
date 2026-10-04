using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    // Total number of waves in this level.
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
    // GAME COMPLETE UI
    // =========================================================

    [Header("Game Complete")]

    // This panel is shown ONLY when there is
    // no next scene in Build Settings.
    //
    // Example:
    //
    // Level-5
    //    ↓
    // No Build Index 6
    //    ↓
    // GameComplete Panel
    //
    [SerializeField] private GameObject gameCompletePanel;


    // =========================================================
    // INTERNAL VARIABLES
    // =========================================================

    // Current wave number.
    private int currentWave = 0;

    // Prevents multiple waves from starting
    // at the same time.
    private bool wavesStarted = false;


    // =========================================================
    // START
    // =========================================================

    private void Start()
    {
        // GameStartTimer has been removed.
        //
        // Gameplay starts immediately.
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

        if (wavesStarted)
        {
            return;
        }


        // Mark wave system as active.
        wavesStarted = true;


        // Move to next wave.
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

        float spawnDelay =
            waveDuration / enemiesPerWave;


        // =====================================================
        // SPAWN ENEMIES
        // =====================================================

        for (int i = 0; i < enemiesPerWave; i++)
        {
            // Select random spawn point.
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


                // Wait before trying again.
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


            // Wait before spawning next enemy.
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


        // Wait until no Enemy objects remain.
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
            // Allow another wave to start.
            wavesStarted = false;


            // Start next wave.
            StartWave();
        }
        else
        {
            // All waves have been completed.
            yield return StartCoroutine(
                CompleteLevel()
            );
        }
    }


    // =========================================================
    // COMPLETE LEVEL
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

        // Your physical coin uses:
        //
        // Tag = Ability_Coin
        //
        // Therefore we wait until all physical coins
        // have been collected.

        Debug.Log(
            "Waiting for remaining coins..."
        );


        while (true)
        {
            // Check whether any physical Ability_Coin
            // remains in the scene.
            bool coinsExist =
                GameObject.FindGameObjectsWithTag(
                    "Ability_Coin"
                ).Length > 0;


            // No coins remaining.
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

        // Find persistent Player.
        GameObject player =
            GameObject.FindWithTag("Player");


        if (player != null)
        {
            PlayerMovement movement =
                player.GetComponent<PlayerMovement>();


            if (movement != null)
            {
                // Stop horizontal movement.
                movement.ForceIdle();


                // Disable movement while the level
                // transition is happening.
                movement.SetMovementEnabled(false);
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

        // Give the player a short moment before
        // loading the next level or showing
        // the Game Complete panel.
        yield return new WaitForSeconds(2f);


        // =====================================================
        // UPDATE HIGH SCORE
        // =====================================================

        // Get saved High Score.
        int previousHighScore =
            PlayerPrefs.GetInt(
                "HighScore",
                0
            );


        // Check whether current score is higher.
        if (ScoreManager.CurrentScore >
            previousHighScore)
        {
            // Save new High Score.
            PlayerPrefs.SetInt(
                "HighScore",
                ScoreManager.CurrentScore
            );


            // Save immediately.
            PlayerPrefs.Save();


            Debug.Log(
                "New High Score: " +
                ScoreManager.CurrentScore
            );
        }


        // =====================================================
        // CHECK NEXT SCENE
        // =====================================================

        int currentSceneIndex =
            SceneManager.GetActiveScene().buildIndex;


        int nextSceneIndex =
            currentSceneIndex + 1;


        Debug.Log(
            "Current Build Index: " +
            currentSceneIndex
        );


        Debug.Log(
            "Next Build Index: " +
            nextSceneIndex
        );


        // =====================================================
        // CHECK IF THIS IS THE FINAL SCENE
        // =====================================================

        // sceneCountInBuildSettings tells us how many
        // scenes are available in Build Settings.
        //
        // Example:
        //
        // 0 Main Menu
        // 1 Level-1
        // 2 Level-2
        // 3 Level-3
        // 4 Level-4
        // 5 Level-5
        //
        // sceneCountInBuildSettings = 6
        //
        // Level-5:
        // currentSceneIndex = 5
        // nextSceneIndex = 6
        //
        // 6 >= 6
        // Therefore there is NO next scene.


        if (
            nextSceneIndex >=
            SceneManager.sceneCountInBuildSettings
        )
        {
            // -------------------------------------------------
            // FINAL LEVEL COMPLETED
            // -------------------------------------------------

            Debug.Log(
                "No next level exists."
            );


            Debug.Log(
                "ALL LEVELS COMPLETED!"
            );


            // -------------------------------------------------
            // SHOW GAME COMPLETE PANEL
            // -------------------------------------------------

            if (gameCompletePanel != null)
            {
                gameCompletePanel.SetActive(true);


                Debug.Log(
                    "Game Complete Panel displayed."
                );
            }
            else
            {
                Debug.LogWarning(
                    "WaveSpawner: Game Complete Panel " +
                    "is not assigned."
                );
            }


            // IMPORTANT:
            //
            // CompleteLevel() is an IEnumerator.
            //
            // Therefore we MUST use:
            //
            // yield break;
            //
            // NOT:
            //
            // return;
            //
            // This ends the coroutine correctly.
            yield break;
        }


        // =====================================================
        // NEXT LEVEL EXISTS
        // =====================================================

        Debug.Log(
            "Next level exists."
        );


        Debug.Log(
            "Loading next level. Build Index: " +
            nextSceneIndex
        );


        // Load next level.
        SceneManager.LoadScene(
            nextSceneIndex
        );
    }
}