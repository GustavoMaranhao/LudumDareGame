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
    public int amountOfKillsRequiredForBoss;

    private float spawn1;
    private float spawn2;
    private float spawn3;
    private float spawn4;
    private float playerStartHeight;

    private bool bSpawnBoss = true;
    private bool bSpawnFinalBoss = false;
    private bool bShouldStopSpawning = false;

    private LevelManager levelManager;
    private GameInitializer gameInitializer;
    private EndGameManager endGameManager;

    private void Awake()
    {
        if (GlobalGameManager.player == null) GlobalGameManager.player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControls>();
        if (GlobalGameManager.uiManager == null) GlobalGameManager.uiManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<UIManager>();

        SetSpawnLimits();
        GlobalEvents.OnWaveStart += StartSpawningEnemies;
        GlobalEvents.OnEnemyDeath += EnemyDeath;

        levelManager = GetComponent<LevelManager>();
        gameInitializer = GetComponent<GameInitializer>();
        endGameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<EndGameManager>();
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

        if (randomValue > 0.25f)
        {
            return new Vector2(spawn1, playerStartHeight);
        }
        else if (randomValue > 0.5f)
        {
            return new Vector2(spawn2, playerStartHeight);
        }
        else if (randomValue > 0.75f)
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
        if (bShouldStopSpawning) return;
        if (bSpawnFinalBoss)
        {
            SpawnBossEnemy(true);
            bShouldStopSpawning = true;
        }

        if (bSpawnBoss)
            SpawnBossEnemy();
        else
            StartCoroutine(StartSpawningWave());
    }

    private void SpawnBossEnemy(bool bFinalBoss = false)
    {
        
		var locationToSpawn = new Vector2(spawn3, playerStartHeight);
		GameObject prefab;
        if (!bFinalBoss)
            prefab = listOfBosses[0];
        else
        {
            prefab = listOfBosses[1];
			locationToSpawn = GetRandomSpawnTransform(1);
        }
        locationToSpawn.y += 10;
        SpawnEnemy(prefab, locationToSpawn);
    }

    private float GetLevelSpawnTimer(int currentLevel)
    {
        if (currentLevel == 1)
        {
            return 7;
        }
        else if (currentLevel > 5)
        {
            return 5;
        }
        else if (currentLevel > 10)
        {
            return 4.5f;
        }
        else if (currentLevel > 20)
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
        for (int i = 0; i < amountOfKillsNecessary; i++)
        {
			int currentLevel = levelManager.GetCurrentLevel();
			 
			for(int j = 0; j < 2; j++){	
				GameObject prefab = GetEnemyPrefab(currentLevel);
				yield return new WaitForSeconds(1.5f);
				SpawnEnemy(prefab, GetRandomSpawnTransform());
				yield return new WaitForSeconds(1);
			}
           
        }
    }

    public int GetAmountOfEnemiesOfLevel()
    {
        //return levelManager.GetCurrentLevel() + 2;
        return 1;
    }

    private GameObject GetEnemyPrefab(int currentLevel)
    {
        int randomValue = UnityEngine.Random.Range(0, 6);

        GameObject prefab = listOfEnemies[randomValue];

        return prefab;
    }

    private void EnemyDeath(object sender, System.EventArgs e)
    {
        EnemyDeathArgs enemyDeathArgs = (EnemyDeathArgs) e;



        if(enemyDeathArgs.tag == "Boss")
        {
            bSpawnBoss = false;
        }

        if (enemyDeathArgs.tag == "FinalBoss")
        {
            endGameManager.gameVictory = true;
            GlobalGameManager.uiManager.toggleGameOverPanel();
        }

        //amountOfEnemiesKilled++;
        if (amountOfEnemiesKilled >= GetAmountOfEnemiesOfLevel())
        {
            gameInitializer.EndWave();
        }
        if(amountOfEnemiesKilled >= amountOfKillsRequiredForBoss)
        {
            bSpawnFinalBoss = true;
        }
    }

    private void SpawnEnemy(GameObject prefab, Vector2 location)
    {
        amountOfEnemiesKilled++;
		GameObject newEnemy = Instantiate(prefab, location, Quaternion.identity);
        BaseEnemy baseEnemy = newEnemy.GetComponent<BaseEnemy>();
        if(!bSpawnBoss) baseEnemy.StartActing();
    }
}
