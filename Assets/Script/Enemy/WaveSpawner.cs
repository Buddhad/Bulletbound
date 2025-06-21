using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class WaveSpawner : MonoBehaviour
{
    public float waveDuration = 10f;
    public int enemiesPerWave = 5;
    public GameObject enemyPrefab;
    public Transform[] spawnPoints;
    [SerializeField] private GameObject LevelCompleteScreen;

    private int currentWave = 0;

    private void OnEnable()
    {
        GameStartTimer.OnGameStarted += StartWave;
    }

    private void OnDisable()
    {
        GameStartTimer.OnGameStarted -= StartWave;
    }

    public void StartWave()
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
            Instantiate(enemyPrefab, spawnPoints[randomIndex].position, Quaternion.identity);
            yield return new WaitForSeconds(waveDuration / enemiesPerWave);
        }

        Debug.Log("Wave " + currentWave + " completed");

        if (currentWave < 5)
        {
            StartWave();
        }
        else
        {
            Debug.Log("All waves completed!");

            while (GameObject.FindGameObjectsWithTag("Enemy").Length > 0)
                yield return null;

            while (true)
            {
                bool abilityExists = false;
                foreach (GameObject obj in GameObject.FindObjectsOfType<GameObject>())
                {
                    if (obj.layer == LayerMask.NameToLayer("Ability"))
                    {
                        abilityExists = true;
                        break;
                    }
                }

                if (GameObject.FindGameObjectsWithTag("Coin").Length == 0 && !abilityExists)
                    break;

                yield return null;
            }

            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                PlayerMovement movement = player.GetComponent<PlayerMovement>();
                if (movement != null)
                {
                    movement.ForceIdle();
                    movement.enabled = false;
                }
            }

            yield return new WaitForSeconds(2f);
            // Update the score display
            // Just before showing level complete screen
            int previousHighScore = PlayerPrefs.GetInt("HighScore", 0);
            if (ScoreManager.CurrentScore > previousHighScore)
            {
                PlayerPrefs.SetInt("HighScore", ScoreManager.CurrentScore);
                PlayerPrefs.Save();
            }
            LevelCompleteScreen.SetActive(true);
        }
    }
}