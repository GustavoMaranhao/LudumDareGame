using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    public GameObject soulItemHUD;
    public GameObject swordItemHUD;
    public GameObject armorItemHUD;

    public GameObject CreditsMenuPanel;
    public GameObject MainMenuPanel;
    public GameObject storyPanelRoot;

    public AudioSource buttonSound;

    private void Start()
    {
        ResetHUD();

        GlobalEvents.OnItemCollected += ReceivedItem;
    }

    private void Destroy()
    {
        GlobalEvents.OnItemCollected -= ReceivedItem;
    }

    private void ReceivedItem(object sender, System.EventArgs e)
    {
        if (!(armorItemHUD || swordItemHUD || soulItemHUD)) return;

        ItemCollectedEventArgs args = (ItemCollectedEventArgs) e;
        EnableItem(args.item);
    }

    public void OnStartStory()
    {
        if (buttonSound != null) buttonSound.Play();
        MainMenuPanel.SetActive(false);
        storyPanelRoot.SetActive(true);
    }

    public void OnTryAgainClick()
    {
        if (buttonSound != null) buttonSound.Play();
        Time.timeScale = 1;
        SceneManager.LoadScene("LudumDareGame", LoadSceneMode.Single);
    }

    public void OnQuitClick()
    {
        if(buttonSound != null) buttonSound.Play();
        Application.Quit();
    }

    public void OnCreditsClickButton()
    {
        if (buttonSound != null) buttonSound.Play();
        MainMenuPanel.SetActive(false);
        CreditsMenuPanel.SetActive(true);
    }

    public void OnCreditsReturnClick()
    {
        if (buttonSound != null) buttonSound.Play();
        MainMenuPanel.SetActive(true);
        CreditsMenuPanel.SetActive(false);
    }

    private void ResetHUD()
    {
        if (!(armorItemHUD || swordItemHUD || soulItemHUD)) return;
        armorItemHUD.SetActive(false);
        swordItemHUD.SetActive(false);
        soulItemHUD.SetActive(false);
    }

    private void EnableItem(ItemType item)
    {
        switch (item)
        {
            case ItemType.Armor:
                armorItemHUD.SetActive(true);
                break;
            case ItemType.Sword:
                swordItemHUD.SetActive(true);
                break;
            case ItemType.Soul:
                soulItemHUD.SetActive(true);
                break;
        }
    }
}
