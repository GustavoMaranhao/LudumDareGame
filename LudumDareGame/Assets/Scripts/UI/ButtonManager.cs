using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    public GameObject soulItem;
    public GameObject swordItem;
    public GameObject armorItem;

    public GameObject CreditsMenuPanel;
    public GameObject MainMenuPanel;

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
        if (!(armorItem || swordItem || soulItem)) return;

        ItemCollectedEventArgs args = (ItemCollectedEventArgs) e;
        EnableItem(args.item);
    }

    public void OnTryAgainClick()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("LudumDareGame", LoadSceneMode.Single);
    }

    public void OnQuitClick()
    {
        Application.Quit();
    }

    public void OnCreditsClickButton()
    {
        MainMenuPanel.SetActive(false);
        CreditsMenuPanel.SetActive(true);
    }

    public void OnCreditsReturnClick()
    {
        MainMenuPanel.SetActive(true);
        CreditsMenuPanel.SetActive(false);
    }

    private void ResetHUD()
    {
        if (!(armorItem || swordItem || soulItem)) return;
        armorItem.SetActive(false);
        swordItem.SetActive(false);
        soulItem.SetActive(false);
    }

    private void EnableItem(ItemType item)
    {
        switch (item)
        {
            case ItemType.Armor:
                armorItem.SetActive(true);
                break;
            case ItemType.Sword:
                swordItem.SetActive(true);
                break;
            case ItemType.Soul:
                soulItem.SetActive(true);
                break;
        }
    }
}
