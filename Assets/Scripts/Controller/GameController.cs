using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameOverView gameOverView;
    [SerializeField] private DifficultySettings fallbackDifficulty;
    [SerializeField] private ColorPalette palette;
    [SerializeField] private GameHudView gameHudView;
    [SerializeField] private BoardView boardView;

    private DifficultySettings settings;
    private readonly System.Random random = new System.Random();
    private readonly RecordStorage recordStorage = new RecordStorage();
    private GameModel model;

    private void Awake()
    {
        boardView.TubeClicked += OnTubeClicked;

        gameOverView.RestartClicked += Restart;

        gameHudView.MenuClicked += GoToMenu;
        gameOverView.MenuClicked += GoToMenu;
    }

    private void Start()
    {
        DifficultySettings difficulty = SceneFlow.SelectedDifficulty;
        if (difficulty == null)
        {
            difficulty = fallbackDifficulty;
        }

        StartGame(difficulty);
    }

    private void Update()
    {
        if (model == null || model.IsOver)
        {
            return;
        }

        model.Tick(Time.deltaTime);
        gameHudView.SetTime(model.TimeLeft);

        if (model.IsTimeUp)
        {
            Lose();
        }
    }

    private void StartGame(DifficultySettings settings)
    {
        gameOverView.Hide();

        this.settings = settings;
        gameHudView.SetDifficulty(settings.difficultyName);

        LevelConfig config = settings.ToConfig();
        Tube[] tubes = LevelGenerator.Generate(config, random);
        model = new GameModel(tubes, config);

        boardView.Build(model.TubeCount, config.TubeCapacity);

        gameHudView.SetTime(model.TimeLeft);
        RenderBoard();
    }

    private void OnTubeClicked(int tubeIndex)
    {
        MoveResult result = model.Click(tubeIndex);

        if(result == MoveResult.None)
        {
            return;
        }

        RenderBoard();

        if (model.IsWon)
        {
            Win();
        }
    }

    private void RenderBoard()
    {
        for(int i = 0; i < model.TubeCount; i++)
        {
            boardView.RenderTube(i, ToColors(model.GetTubeColors(i)));
        }

        boardView.SetSelected(model.SelectedIndex);
    }

    private void Win()
    {
        float? previousBest = recordStorage.Load(settings.id);

        if (model.IsNewRecord(previousBest))
        {
            recordStorage.Save(settings.id, model.TimeLeft);
        }

        gameOverView.ShowWin();
    }

    private void Lose()
    {
        gameOverView.ShowLose();
    }

    private void GoToMenu()
    {
        SceneFlow.LoadMenu();
    }

    private void Restart()
    {
        StartGame(settings);
    }

    private Color[] ToColors(IReadOnlyList<int> colorIds)
    {
        Color[] result = new Color[colorIds.Count];

        for (int i = 0; i < colorIds.Count; i++)
        {
            result[i] = palette.GetColor(colorIds[i]);
        }

        return result;
    }
}