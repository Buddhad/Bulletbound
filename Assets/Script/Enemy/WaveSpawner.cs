using System.Collections;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    [Header("Wave Settings")]
    public float waveDuration = 10f;
    public int enemiesPerWave = 5;
    public int totalWaves = 5;

    [Header("Enemy")]
    public GameObject enemyPrefab;
    public Transform[] spawnPoints;

    [Header("Level Complete")]
    [SerializeField] private GameObject LevelCompleteScreen;

    private int currentWave = 0;
    private bool wavesStarted = false;

    private void OnEnable()
    {
        GameStartTimer.OnGameStarted += StartWave;
    }

    private void OnDisable()
    {
        GameStartTimer.OnGameStarted -= StartWave;
    }

    private void Start()
    {
        // Safety for Debug Start Immediately or
        // starting the Gameplay scene directly.
        if (GameStartTimer.GameStarted && !wavesStarted)
        {
            StartWave();
        }
    }

    public void StartWave()
    {
        // Prevent the event and Start() from starting
        // the first wave twice.
        if (wavesStarted && currentWave >= totalWaves)
            return;

        if (currentWave >= totalWaves)
            return;

        wavesStarted = true;

        currentWave++;

        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        Debug.Log("Starting wave " + currentWave);

        // Safety check
        if (enemyPrefab == null)
        {
            Debug.LogError("WaveSpawner: Enemy Prefab is not assigned!");
            yield break;
        }

        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogError("WaveSpawner: No spawn points assigned!");
            yield break;
        }

        float spawnDelay = waveDuration / enemiesPerWave;

        for (int i = 0; i < enemiesPerWave; i++)
        {
            int randomIndex =
                Random.Range(0, spawnPoints.Length);

            Instantiate(
                enemyPrefab,
                spawnPoints[randomIndex].position,
                Quaternion.identity
            );

            yield return new WaitForSeconds(spawnDelay);
        }

        Debug.Log("Wave " + currentWave + " spawned.");

        // Wait until all enemies from this wave are dead
        while (GameObject.FindGameObjectsWithTag("Enemy").Length > 0)
        {
            yield return null;
        }

        Debug.Log("Wave " + currentWave + " completed.");

        // Start next wave
        if (currentWave < totalWaves)
        {
            StartWave();
        }
        else
        {
            yield return StartCoroutine(CompleteLevel());
        }
    }

    private IEnumerator CompleteLevel()
    {
        Debug.Log("All waves completed!");

        // Wait for abilities and coins to disappear
        while (true)
        {
            bool abilityExists = false;

            GameObject[] objects =
                GameObject.FindObjectsByType<GameObject>(
                    FindObjectsSortMode.None
                );

            int abilityLayer =
                LayerMask.NameToLayer("Ability");

            foreach (GameObject obj in objects)
            {
                if (obj.layer == abilityLayer)
                {
                    abilityExists = true;
                    break;
                }
            }

            bool coinsExist =
                GameObject.FindGameObjectsWithTag("Coin").Length > 0;

            if (!coinsExist && !abilityExists)
                break;

            yield return null;
        }

        // Stop player
        GameObject player =
            GameObject.FindWithTag("Player");

        if (player != null)
        {
            PlayerMovement movement =
                player.GetComponent<PlayerMovement>();

            if (movement != null)
            {
                movement.ForceIdle();
                movement.enabled = false;
            }
        }

        // Small delay before level complete screen
        yield return new WaitForSeconds(2f);

        // Update High Score
        int previousHighScore =
            PlayerPrefs.GetInt("HighScore", 0);

        if (ScoreManager.CurrentScore > previousHighScore)
        {
            PlayerPrefs.SetInt(
                "HighScore",
                ScoreManager.CurrentScore
            );

            PlayerPrefs.Save();

            Debug.Log(
                "New High Score: " +
                ScoreManager.CurrentScore
            );
        }

        // Show level complete UI
        if (LevelCompleteScreen != null)
        {
            LevelCompleteScreen.SetActive(true);
        }
        else
        {
            Debug.LogWarning(
                "LevelCompleteScreen is not assigned."
            );
        }
    }
}