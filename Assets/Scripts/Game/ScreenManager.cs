using UnityEngine;

public class ScreenManager : MonoBehaviour
{

    public GameObject menuPanel;
    public GameObject gamePanel;
    public GameObject winPanel;
    public GameObject losePanel;

    public void ShowMenuPanel()
    {
        menuPanel.SetActive(true);
        winPanel.SetActive(false);
        losePanel.SetActive(false);
        gamePanel.SetActive(false);
    }
    public void ShowGamePanel()
    {
        menuPanel.SetActive(false);
        gamePanel.SetActive(true);
        winPanel.SetActive(false);
        losePanel.SetActive(false);
    }
    public void ShowWinPanel()
    {
        winPanel.SetActive(true);
    }
    public void ShowLosePanel()
    {
        losePanel.SetActive(true);
    }

    private void Start()
    {
        ShowMenuPanel();
    }
}
