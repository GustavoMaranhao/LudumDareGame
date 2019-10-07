using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject gameOverPanel;
    public GameObject gameOverTextPanel;
    public GameObject gameVictoryTextPanel;
    public Text victoryScoreText;

    public List<GameObject> elementsToDeactivate;

    private EndGameManager endGameManager;
    private float gameEndingTime = 0f;

    private void Start()
    {
        gameOverPanel.SetActive(true);
        gameOverTextPanel.SetActive(true);
        gameOverPanel = GameObject.FindGameObjectWithTag("GameOverPanel");
        gameOverTextPanel = GameObject.FindGameObjectWithTag("GameOverText");
        gameOverTextPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        gameVictoryTextPanel.SetActive(false);

        endGameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<EndGameManager>();
    }

    public void toggleGameOverPanel()
    {
        gameEndingTime = Time.realtimeSinceStartup;
        StartCoroutine(TimeToPause());
        gameOverPanel.SetActive(true);
        if(elementsToDeactivate.Count > 0)
        {
            foreach(GameObject item in elementsToDeactivate)
            {
                item.SetActive(false);
            }
        }
    }

    IEnumerator TimeToPause()
    {
        yield return new WaitForSeconds(2);
        Time.timeScale = 0;
    }

    private void Update()
    {
        if (gameOverPanel.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("UIPanelFadeOut"))
        {
            if(endGameManager.gameOver)
                gameOverTextPanel.SetActive(true);

            if (endGameManager.gameVictory)
            {
                SetScoreText(gameEndingTime.ToString());
                gameVictoryTextPanel.SetActive(true);
            }
        }
    }

    public void SetScoreText(string text)
    {
        victoryScoreText.text = text;
    }
}
