using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverPopup : MonoBehaviour
{
    [SerializeField] private GameController controller;
    [SerializeField] private GameObject window;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button nextLevelButton;
    [SerializeField] private Button menuButton;

    private void Awake()
    {
        restartButton.onClick.AddListener(() => controller.Restart());
        nextLevelButton.onClick.AddListener(() => controller.NextLevel());
        menuButton.onClick.AddListener(() => controller.GoToMenu());

        controller.GameWon += OnGameWon;
        controller.GameLost += OnGameLost;
        controller.LevelStarted += OnLevelStarted;

        window.SetActive(false);
    }

    private void OnDestroy()
    {
        controller.GameWon -= OnGameWon;
        controller.GameLost -= OnGameLost;
        controller.LevelStarted -= OnLevelStarted;
    }

    private void OnGameWon()
    {
        Show(true);
    }

    private void OnGameLost()
    {
        Show(false);
    }

    private void OnLevelStarted()
    {
        window.SetActive(false);
    }

    private void Show(bool isWon)
    {
        if(isWon)
        {
            text.text = "You Win!";
            text.color = Color.green;
            restartButton.gameObject.SetActive(false);
            nextLevelButton.gameObject.SetActive(controller.HasNextLevel);
        }
        else
        {
            text.text = "Time is up";
            text.color = Color.red;
            restartButton.gameObject.SetActive(true);
            nextLevelButton.gameObject.SetActive(false);
        }

        window.SetActive(true);
    }    
}