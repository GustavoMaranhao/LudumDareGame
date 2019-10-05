using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private int currentLevel;

    private void Awake()
    {
        currentLevel = 1;
    }

    public void AddLevel()
    {
        currentLevel++;
    }

    public int GetCurrentLevel()
    {
        return currentLevel;
    }
}
