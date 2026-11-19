using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameHudView : MonoBehaviour
{
    [SerializeField] private GameController controller;
    [SerializeField] private TextMeshProUGUI difficultyText;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private Button menuButton;

    private void Awake()
    {
        menuButton.onClick.AddListener(() => controller.GoToMenu());
        controller.LevelStarted += OnLevelStarted;
        controller.TimeChanged += OnTimeChanged;
    }
    
    private void OnDestroy()
    {
        controller.LevelStarted -= OnLevelStarted;
        controller.TimeChanged -= OnTimeChanged;
    }

    private void OnLevelStarted()
    {
        difficultyText.text = controller.LevelName;
    }

    private void OnTimeChanged(int seconds)
    {
        timerText.text = TimeFormat.ToMinutesSeconds(seconds);
    }
}