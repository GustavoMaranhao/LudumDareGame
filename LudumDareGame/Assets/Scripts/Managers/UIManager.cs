using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject gameOverPanel;
    public GameObject textPanel;

    private void Start()
    {
        gameOverPanel.SetActive(true);
        textPanel.SetActive(true);
        gameOverPanel = GameObject.FindGameObjectWithTag("GameOverPanel");
        textPanel = GameObject.FindGameObjectWithTag("GameOverText");
        textPanel.SetActive(false);
        gameOverPanel.SetActive(false);
    }

    public void toggleGameOverPanel()
    {
        gameOverPanel.SetActive(true);
    }

    private void Update()
    {
        if (!gameOverPanel.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("UIPanelFadeOut"))
            textPanel.SetActive(true);
    }
}
