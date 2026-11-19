using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameHudView : MonoBehaviour
{
    [SerializeField] private GameController controller;
    [SerializeField] private TextMeshProUGUI difficultyText;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private Button menuButton;

    private string prevTimeLeft = string.Empty;

    private void Awake()
    {
        menuButton.onClick.AddListener(() => controller.GoToMenu());
        controller.LevelStarted += OnLevelStarted;
    }
    
    private void Update()
    {
        var gameModel = controller.Model;
        string timeLeft = TimeFormat.ToMinutesSeconds(gameModel.TimeLeft);
        if (prevTimeLeft != timeLeft)
        {
            prevTimeLeft = timeLeft;
            timerText.text = timeLeft;
        }
    }

    private void OnDestroy()
    {
        controller.LevelStarted -= OnLevelStarted;
    }

    private void OnLevelStarted()
    {
        difficultyText.text = controller.LevelName;
    }
}