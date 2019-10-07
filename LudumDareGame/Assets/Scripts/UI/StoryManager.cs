using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StoryManager : MonoBehaviour
{
    public GameObject storyPanel1;
    public GameObject storyPanel2;
    public GameObject storyPanel3;
    public GameObject nextButton;
    public GameObject startButton;
    public AudioSource buttonSound;

    public void OnNextClick()
    {
        if (buttonSound != null) buttonSound.Play();

        if (storyPanel1.activeInHierarchy)
        {
            storyPanel1.SetActive(false);
            storyPanel2.SetActive(true);
            storyPanel3.SetActive(false);
        }
        else if (storyPanel2.activeInHierarchy)
        {
            storyPanel1.SetActive(false);
            storyPanel2.SetActive(false);
            storyPanel3.SetActive(true);
            nextButton.SetActive(false);
            startButton.SetActive(true);
        }
    }

    public void OnBeginClick()
    {
        if (buttonSound != null) buttonSound.Play();
        Time.timeScale = 1;
        SceneManager.LoadScene("LudumDareGame", LoadSceneMode.Single);
    }
}
