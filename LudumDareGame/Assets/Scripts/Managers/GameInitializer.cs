using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    LevelManager levelManager;
    private void Awake()
    {
        levelManager = FindObjectOfType<LevelManager>();
        FloatingTextController.Initialize();
    }

    void Start()
    {
        StartNextWave();
    }

    private void StartNextWave()
    {
        GlobalEvents.WaveStart(this, null);
    }

    public void EndWave()
    {
        StartCoroutine(NextWave());
    }

    public IEnumerator NextWave()
    {
        //levelManager.AddLevel();
        yield return new WaitForSeconds(8);
        StartNextWave();
    }
}
