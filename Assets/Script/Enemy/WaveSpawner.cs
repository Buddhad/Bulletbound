using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaveSpawner : MonoBehaviour
{
    public float waveDuration = 10f;
    public int enemiesPerWave = 5;
    public GameObject enemyPrefab;
    public Transform[] spawnPoints;
    [SerializeField] private GameObject LevelCompleteScreen;
    private int currentWave = 0;

    void Start()
    {
        StartWave();
    }

    void StartWave()
    {
        currentWave++;
        StartCoroutine(SpawnEnemies());

    }

    IEnumerator SpawnEnemies()
    {
        Debug.Log("Starting wave " + currentWave);

        for (int i = 0; i < enemiesPerWave; i++)
        {
            int randomIndex = Random.Range(0, spawnPoints.Length);
            // Instantiate the enemy prefab
            GameObject enemyInstance = Instantiate(enemyPrefab, spawnPoints[randomIndex].position, Quaternion.identity);
            // Add the EnemyFollow script if needed
            //enemyInstance.AddComponent<EnemyFollow>();
            yield return new WaitForSeconds(waveDuration / enemiesPerWave);
        }
        Debug.Log("Wave " + currentWave + " completed");
        // Check if there are more waves to spawn
        if (currentWave < 3) // Assuming you want 3 waves
        {
            StartWave();
        }
        else
        {
            Debug.Log("All waves completed!");

            // Wait until all enemies are destroyed
            while (GameObject.FindGameObjectsWithTag("Enemy").Length > 0)
            {
                yield return null;
            }

            // 🪙 Wait until all coins are collected
            while (GameObject.FindGameObjectsWithTag("Coin").Length > 0)
            {
                yield return null;
            }

            // Disable player movement and force idle
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                PlayerMovement movement = player.GetComponent<PlayerMovement>();
                if (movement != null)
                {
                    movement.ForceIdle();           // ✅ stop movement visually
                    movement.enabled = false;       // then disable control
                }
            }

            // Short delay before showing level complete UI
            yield return new WaitForSeconds(2f);
            LevelCompleteScreen.SetActive(true);

            /*
            // Start the next wave after a delay (optional)
            yield return new WaitForSeconds(waveDuration);
            StartWave();
            */
        }
    }

}