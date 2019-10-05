using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public List<GameObject> listOfEnemies;
    public List<Transform> spawnLimits;

    public int amountOfKillsNecessary;
    public int amountOfEnemiesKilled;

    private float rightLimit;
    private float leftLimit;
    private float topLimit;
    private float bottomLimit;

    private LevelManager levelManager;
    private GameInitializer gameInitializer;

    private void Awake()
    {
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
        rightLimit = spawnLimits[0].position.x;
        leftLimit = spawnLimits[1].position.x;
        topLimit = spawnLimits[2].position.y;
        bottomLimit = spawnLimits[3].position.y;
    }

    public Vector2 GetRandomSpawnTransform()
    {
        float randomValue = UnityEngine.Random.Range(0f, 1f);
        // Right
        if (randomValue < 0.5f)
        {
            return new Vector2(rightLimit, UnityEngine.Random.Range(bottomLimit, topLimit));
        }
        // Left
        else
        {
            return new Vector2(leftLimit, UnityEngine.Random.Range(bottomLimit, topLimit));
        }
    }

    public void StartSpawningEnemies(object sender, System.EventArgs e)
    {
        amountOfEnemiesKilled = 0;
        StartCoroutine(StartSpawningWave());
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
            SpawnEnemy(prefab);
            yield return new WaitForSeconds(GetLevelSpawnTimer(currentLevel));
        }
    }

    public int GetAmountOfEnemiesOfLevel()
    {
        return levelManager.GetCurrentLevel() + 2;
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
        amountOfEnemiesKilled++;
        if (amountOfEnemiesKilled == GetAmountOfEnemiesOfLevel())
        {
            gameInitializer.EndWave();
        }
    }

    private void SpawnEnemy(GameObject prefab)
    {
        GameObject newEnemy = Instantiate(prefab, GetRandomSpawnTransform(), Quaternion.identity);
        BaseEnemy baseEnemy = newEnemy.GetComponent<BaseEnemy>();
        baseEnemy.StartActing();
    }
}
