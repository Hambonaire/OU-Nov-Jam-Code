using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct WaveEnemy
{
    public GameObject enemyMinionPrefab;

    // 0 - 5
    // Will factor in to when this minion begins spawning and how many spawn in each wave.
    public int tier;

    // The first wave this minion can start spawning on
    public int waveBreakpoint;

    // The minimum & normal spawn rate of the minion
    public Vector2 spawnRate;

    public int a;
    public int b;
}

public class WaveManager : MonoBehaviour
{
    public static WaveManager _instance;
    GameManager gameManager;
    UIManager uiManager;

    public Transform[] pathWaypoints;

    public List<WaveEnemy> waveEnemies = new List<WaveEnemy>();

    public int currentWave = -1;
    public bool waveActive = false;

    public List<GameObject> aliveEnemies = new List<GameObject>();
    public List<GameObject> aliveAllies = new List<GameObject>();

    float lastSpawnTime;

    void Awake()
    {
        _instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager._instance;
        uiManager = UIManager._instance;

        StartNextWave();
    }

    // Update is called once per frame
    void Update()
    {
        if (waveActive)
            CheckWaveOver();
    }

    public void StartNextWave()
    {
        currentWave++;

        SpawnMinions();
    }    

    public void CheckWaveOver()
    {
        if (waveActive && Time.time > lastSpawnTime && aliveEnemies.Count == 0)
        {
            gameManager.AddRelicToInventory(new Relic());

            Invoke("StartNextWave", 5f);
            waveActive = false;
        }
    }

    public void SpawnMinions()
    {
        waveActive = true;

        foreach (WaveEnemy enemy in waveEnemies)
        {
            if (enemy.waveBreakpoint > currentWave)
                continue;


            int countToSpawn = Mathf.CeilToInt(Mathf.Sqrt(enemy.a * currentWave) + enemy.b);
            //int countToSpawn = Mathf.CeilToInt(Mathf.Sqrt(20 * currentWave) + 10);
            //countToSpawn = Mathf.CeilToInt(Mathf.Sqrt(52 * currentWave) - 12);

            float totalTime = 0;
            for (int cnt = 0; cnt < countToSpawn; cnt++)
            {
                totalTime += Random.Range(enemy.spawnRate.x, enemy.spawnRate.y);
                StartCoroutine(SpawnAt(enemy.enemyMinionPrefab, totalTime));

                if (totalTime > lastSpawnTime) lastSpawnTime = totalTime;
            }
        }    
    }

    IEnumerator SpawnAt(GameObject enemy, float wait)
    {
        yield return new WaitForSeconds(wait);

        Instantiate(enemy, pathWaypoints[0].position, pathWaypoints[0].rotation);
    }

    public void Reset()
    {
        StopAllCoroutines();

        currentWave = -1;
        waveActive = false;

        for(int i = aliveAllies.Count - 1; i >= 0; i--)
            if (aliveAllies[i].GetComponent<Minion>().isGate) 
                Destroy(aliveAllies[i].gameObject);
        aliveAllies.Clear();

        for (int i = aliveEnemies.Count - 1; i >= 0; i--)
            Destroy(aliveEnemies[i].gameObject);
        aliveEnemies.Clear();

        Invoke("StartNextWave", 5f);
    }
}
