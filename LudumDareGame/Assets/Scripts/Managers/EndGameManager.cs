using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameManager : MonoBehaviour
{
    public bool gameOver = false;
    public bool gameVictory = false;

    private void Start()
    {
        gameOver = false;
        gameVictory = false;
    }
}
