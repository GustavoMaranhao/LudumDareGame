using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    public void OnTryAgainClick()
    {
        SceneManager.LoadScene("LudumDareGame", LoadSceneMode.Single);
    }
}
