using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public List<GameObject> listOfEnemies;
    public List<GameObject> listOfBosses;
    public List<Transform> spawnLimits;

    public int amountOfKillsNecessary;
    public int amountOfEnemiesKilled;

    private float spawn1;
    private float spawn2;
    private float spawn3;
    private float spawn4;
    private float playerStartHeight;

    private bool bSpawnBoss = true;

    private LevelManager levelManager;
    private GameInitializer gameInitializer;

    private void Awake()
    {
        if (GlobalGameManager.player == null) GlobalGameManager.player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControls>();
        if (GlobalGameManager.uiManager == null) GlobalGameManager.uiManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<UIManager>();

        SetSpawnLimits();
        GlobalEvents.OnWaveStart += StartSpawningEnemies;
        GlobalEvents.OnEnemyDeath += EnemyDeath;

        levelManager = GetComponent<LevelManager>();
        gameInitializer = GetComponent<GameInitializer>();
    }


    private void OnDestroy()
    {
        GlobalEvents.OnEnemyDeath -= EnemyDeath;
        GlobalEvents.OnWaveStart -= StartSpawningEnemies;
    }

    private void SetSpawnLimits()
    {
        spawn1 = spawnLimits[0].position.x;
        spawn2 = spawnLimits[1].position.x;
        spawn3 = spawnLimits[2].position.x;
        spawn4 = spawnLimits[3].position.x;
        playerStartHeight = GlobalGameManager.player.transform.position.y;
    }

    public Vector2 GetRandomSpawnTransform(float forceValue = 0f)
    {
        float randomValue = 0f;
        if (forceValue != 0) randomValue = forceValue;
        else randomValue = UnityEngine.Random.Range(0f, 1f);

        if (randomValue < 0.25f)
        {
            return new Vector2(spawn1, playerStartHeight);
        }
        else if (randomValue < 0.5f)
        {
            return new Vector2(spawn2, playerStartHeight);
        }
        else if (randomValue < 0.75f)
        {
            return new Vector2(spawn3, playerStartHeight);
        }
        else
        {
            return new Vector2(spawn4, playerStartHeight);
        }
    }

    public void StartSpawningEnemies(object sender, System.EventArgs e)
    {
        amountOfEnemiesKilled = 0;
        if (bSpawnBoss)
            SpawnBossEnemy();
        else
            StartCoroutine(StartSpawningWave());
    }

    private void SpawnBossEnemy()
    {
        GameObject prefab = listOfBosses[0];
        var locationToSpawn = GetRandomSpawnTransform(1);
        locationToSpawn.y += 10;
        SpawnEnemy(prefab, locationToSpawn);
    }

    private float GetLevelSpawnTimer(int currentLevel)
    {
        if (currentLevel < 5)
        {
            return 5;
        }
        else if (currentLevel < 10)
        {
            return 4.5f;
        }
        else if (currentLevel < 20)
        {
            return 3;
        }
        else
        {
            return 2;
        }
    }

    IEnumerator StartSpawningWave()
    {
        amountOfKillsNecessary = GetAmountOfEnemiesOfLevel();
        for (int i = 0; i < GetAmountOfEnemiesOfLevel(); i++)
        {
            int currentLevel = levelManager.GetCurrentLevel();
            GameObject prefab = GetEnemyPrefab(currentLevel);
            SpawnEnemy(prefab, GetRandomSpawnTransform());
            yield return new WaitForSeconds(GetLevelSpawnTimer(currentLevel));
        }
    }

    public int GetAmountOfEnemiesOfLevel()
    {
        //return levelManager.GetCurrentLevel() + 2;
        return 1;
    }

    private GameObject GetEnemyPrefab(int currentLevel)
    {
        float randomValue = UnityEngine.Random.Range(0f, 1f);

        GameObject prefab;

        if (randomValue < 0.05)
        {
            prefab = listOfEnemies[3];
        }
        else if (randomValue < 0.1)
        {
            prefab = listOfEnemies[4];
        }
        else if (randomValue < 0.15)
        {
            prefab = listOfEnemies[5];
        }
        else if (randomValue < 0.4)
        {
            prefab = listOfEnemies[1];
        }
        else if (randomValue < 0.7)
        {
            prefab = listOfEnemies[2];
        }
        else
        {
            prefab = listOfEnemies[0];
        }

        return prefab;
    }

    private void EnemyDeath(object sender, System.EventArgs e)
    {
        EnemyDeathArgs enemyDeathArgs = (EnemyDeathArgs) e;

        if(enemyDeathArgs.tag == "Boss")
        {
            bSpawnBoss = false;
        }

        amountOfEnemiesKilled++;
        if (amountOfEnemiesKilled == GetAmountOfEnemiesOfLevel())
        {
            gameInitializer.EndWave();
        }
    }

    private void SpawnEnemy(GameObject prefab, Vector2 location)
    {
        GameObject newEnemy = Instantiate(prefab, location, Quaternion.identity);
        BaseEnemy baseEnemy = newEnemy.GetComponent<BaseEnemy>();
        if(!bSpawnBoss) baseEnemy.StartActing();
    }
}
