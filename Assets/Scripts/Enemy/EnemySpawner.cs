using System;
using System.Collections;
using TMPro;
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
    public int CurrentWave => currentWave; // Ajoute cette ligne pour lire la vague depuis l'extérieur
    
    [Header("Wave Scaling")]
    [SerializeField] private int baseWaveEnemies = 5;
    [SerializeField] private int enemiesIncreasePerWave = 1;

    // Buffs progressifs (raisonnables)
    [SerializeField] private float healthMultPerWave = 1.15f;  // +15% / wave
    [SerializeField] private float damageMultPerWave = 1.10f;  // +10% / wave
    [SerializeField] private float speedMultPerWave  = 1.06f;  // +6% / wave

    
    [Header("References")]
    [SerializeField] private WaveMessageUI waveText;
    
    Camera cam;

    void Start()
    {
        currentWave = 1;
        enemyAlive = 0;
        currentWaveEnemie = baseWaveEnemies;
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

        GameObject go = Instantiate(prefab, pos, Quaternion.identity);

        EnemyController ec = go.GetComponent<EnemyController>();
        if (ec != null)
        {
            ApplyWaveScaling(ec);
        }
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
        waveText.SetWave(currentWave);
        waveText.ShowWave(currentWave);
        enemyAlive = 0;
        enemySpawned = 0;
        currentWaveEnemie += enemiesIncreasePerWave;
    }
    
    private void ApplyWaveScaling(EnemyController enemy)
    {
        // wave 1 = pas de buff (mult = 1)
        int wave = Mathf.Max(1, currentWave);

        float incrementVal = Mathf.Sqrt(wave - 1);
        
        float healthMult = Mathf.Pow(healthMultPerWave, incrementVal);
        float damageMult = Mathf.Pow(damageMultPerWave, incrementVal);
        float speedMult  = Mathf.Pow(speedMultPerWave,  incrementVal);

        enemy.maxHealth *= healthMult;
        enemy.damage    *= damageMult;
        enemy.speed     *= speedMult;

        enemy.currentHealth = enemy.maxHealth;
    }

}