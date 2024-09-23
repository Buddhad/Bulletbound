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
    //[SerializeField]private GameObject LevelCompleteScreen;

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
        

        // Start the next wave after a delay (optional)
        yield return new WaitForSeconds(waveDuration);
        StartWave();
    }
}
