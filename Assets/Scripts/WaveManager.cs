using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemySend
{
    public GameObject enemyPrefab;
    public int numberOfEnemies;
    public float spacing;
    public float timeBeforeNextSend;
}

[System.Serializable]
public class Wave
{
    public List<EnemySend> enemySends = new List<EnemySend>();
}

public class WaveManager : MonoBehaviour
{
    public Wave[] waves; // Define your waves in the Inspector

    public GameObject waypointParent;
    private GameObject startPoint;

    private int currentWaveIndex = 0;
    private bool startNextWave = false;
    private bool firstWave = true;
    public bool stopSpawningWaves = false;

    private void Start()
    {
        // Check if there are any child objects under waypointParent
        if (waypointParent.transform.childCount > 0)
        {
            // Get the first child object as the startPoint
            startPoint = waypointParent.transform.GetChild(0).gameObject;
        }
    }

    void Update()
    {
        // Check if the next wave should be sent
        if (startNextWave)
        {
            StartWave();
        }
    }

    public void StartNextWaveButton()
    {
        startNextWave = true;
    }

    void StartWave()
    {
        if (stopSpawningWaves)
        {
            startNextWave = false;
            Debug.Log("All waves completed!");
            return;
        }

        if (!firstWave)
        {
            currentWaveIndex++;
        }
        else
        {
            firstWave = false;
        }
        
        if (currentWaveIndex < waves.Length)
        {
            startNextWave = false;
            StartCoroutine(SpawnWave(waves[currentWaveIndex]));
        }
        else
        {
            startNextWave = false;
            Debug.Log("All waves completed!");
            stopSpawningWaves = true;
        }
    }

    public int GetCurrentWaveIndex()
    {
        return currentWaveIndex;
    }

    public int GetTotalWaves()
    {
        return waves.Length;
    }

    IEnumerator SpawnWave(Wave wave)
    {
        foreach (EnemySend send in wave.enemySends)
        {
            StartCoroutine(SpawnEnemies(send));
            yield return new WaitForSeconds(send.timeBeforeNextSend);
        }
    }

    IEnumerator SpawnEnemies(EnemySend send)
    {
        for (int i = 0; i < send.numberOfEnemies; i++)
        {
            Instantiate(send.enemyPrefab, startPoint.transform.position, Quaternion.identity);

            yield return new WaitForSeconds(send.spacing);
        }
    }
}