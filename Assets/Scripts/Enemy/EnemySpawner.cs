using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    public float spawnInterval = 2f;
    public float margin = 0.1f;
    private int enemyAlive;
    private int enemySpawned;
    private int currentWave;
    private int currentWaveEnemie;
    private bool isWaveTransitioning = false;
    
    Camera cam;

    void Start()
    {
        currentWave = 1;
        enemyAlive = 0;
        currentWaveEnemie = 5;
        cam = Camera.main;
        StartCoroutine(SpawnLoop());
    }
    
    IEnumerator SpawnLoop()
    {
        while (true)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnEnemy()
    {
        if (enemyPrefabs.Length == 0 || cam == null) return;

        if (enemySpawned >= currentWaveEnemie) return;
        GameObject prefab =
            enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];

        Vector2 pos = GetOutsideFOVPosition();

        Instantiate(prefab, pos, Quaternion.identity);
        enemySpawned++;
        enemyAlive++;
    }

    Vector2 GetOutsideFOVPosition()
    {
        int side = Random.Range(0, 4);

        float x = Random.Range(0f, 1f);
        float y = Random.Range(0f, 1f);

        // force un côté hors écran
        if (side == 0) x = -margin;        
        else if (side == 1) x = 1f + margin; 
        else if (side == 2) y = 1f + margin; 
        else y = -margin;                   

        float z = Mathf.Abs(cam.transform.position.z);
        return cam.ViewportToWorldPoint(new Vector3(x, y, z));
    }

    public void OnEnemyKilled()
    {
        enemyAlive--;
        if (!isWaveTransitioning &&
            enemySpawned >= currentWaveEnemie &&
            enemyAlive <= 0)
        {
            isWaveTransitioning = true;
            StartCoroutine(NextWaveAfterDelay());
        }
    }
    
    IEnumerator NextWaveAfterDelay()
    {
        yield return new WaitForSeconds(1f);
        StartNextWave();
        isWaveTransitioning = false;
    }


    public void StartNextWave()
    {
        Debug.Log("Starting wave " + currentWave);
        currentWave++;
        enemyAlive = 0;
        enemySpawned = 0;
        currentWaveEnemie += 2;
    }
}