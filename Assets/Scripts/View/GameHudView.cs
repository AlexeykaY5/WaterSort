using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class GameHudView : MonoBehaviour
{
    public event Action MenuClicked;

    [SerializeField] private TextMeshProUGUI difficultyText;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private Button menuButton;

    private IGameState state;
    private ConfigDatabase configs;
    private LevelSelection levelSelection;
    private int shownSeconds = -1;

    [Inject]
    public void Construct(IGameState state, ConfigDatabase configs, LevelSelection levelSelection)
    {
        this.state = state;
        this.configs = configs;
        this.levelSelection = levelSelection;
    }

    private void Awake()
    {
        menuButton.onClick.AddListener(() => MenuClicked?.Invoke());
    }

    private void Start()
    {
        difficultyText.text = configs.GetDifficulty(levelSelection.SelectedId).DifficultyName;
    }

    private void Update()
    {
        int wholeSeconds = Mathf.CeilToInt(state.TimeLeft);
        if (wholeSeconds == shownSeconds)
        {
            return;
        }

        shownSeconds = wholeSeconds;
        timerText.text = TimeFormat.ToMinutesSeconds(state.TimeLeft);
    }
}