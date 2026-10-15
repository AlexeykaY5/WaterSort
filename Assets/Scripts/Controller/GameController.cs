using UnityEngine;
using Zenject;

public class GameController : MonoBehaviour
{
    [SerializeField] private BoardView boardView;
    [SerializeField] private GameHudView gameHudView;
    [SerializeField] private GameOverView gameOverView;

    private GameModel model;
    private RecordStorage recordStorage;
    private ConfigDatabase configs;
    private LevelSelection levelSelection;
    private SceneLoader sceneLoader;
    private ColorPalette palette;

    private DifficultySettings settings;
    private readonly System.Random random = new System.Random();

    [Inject]
    public void Construct(GameModel model, RecordStorage recordStorage, ConfigDatabase configs,
                          ColorPalette palette, LevelSelection levelSelection, SceneLoader sceneLoader)
    {
        this.model = model;
        this.recordStorage = recordStorage;
        this.configs = configs;
        this.levelSelection = levelSelection;
        this.sceneLoader = sceneLoader;
        this.palette = palette;
    }

    private void Awake()
    {
        boardView.TubeClicked += OnTubeClicked;

        gameOverView.RestartClicked += Restart;

        gameHudView.MenuClicked += GoToMenu;
        gameOverView.MenuClicked += GoToMenu;
    }

    private void Start()
    {
        settings = configs.GetDifficulty(levelSelection.SelectedId);
        StartGame();
    }

    private void Update()
    {
        if (model.IsOver)
        {
            return;
        }

        model.TimeLeft -= Time.deltaTime;
        if (model.TimeLeft <= 0f)
        {
            model.TimeLeft = 0f;
            model.IsTimeUp = true;
        }
    }

    private void StartGame()
    {
        LevelGenerator.Generate(model.Tubes, settings, palette, random);
        model.SelectedIndex = -1;
        model.TimeLeft = settings.StartTime;
        model.IsWon = false;
        model.IsTimeUp = false;
        model.CompletedColors.Clear();
        model.Round++;
    }

    private void OnTubeClicked(int index)
    {
        if (model.IsOver)
        {
            return;
        }

        if (model.SelectedIndex == -1)
        {
            if (model.Tubes[index].IsEmpty)
            {
                return;
            }
            model.SelectedIndex = index;
        }
        else if (model.SelectedIndex == index)
        {
            model.SelectedIndex = -1;
        }
        else
        {
            Pour(model.SelectedIndex, index);
            model.SelectedIndex = -1;
        }

        if (model.IsWon)
        {
            SaveRecord();
        }
    }

    private void Pour(int fromIndex, int toIndex)
    {
        int poured = model.Tubes[fromIndex].PourInto(model.Tubes[toIndex]);
        if (poured == 0)
        {
            return;
        }

        model.IsWon = CheckWin();
        if (!model.IsWon)
        {
            TryGiveBonus(toIndex);
        }
    }

    private void TryGiveBonus(int index)
    {
        Tube tube = model.Tubes[index];
        if (!tube.IsSingleColor())
        {
            return;
        }

        string color = tube.TopColor();
        if (model.CompletedColors.Add(color))
        {
            model.TimeLeft += settings.BonusTime;
        }
    }

    private bool CheckWin()
    {
        foreach (Tube tube in model.Tubes)
        {
            if (!tube.IsSingleColor() && !tube.IsEmpty)
            {
                return false;
            }
        }
        return true;
    }

    private void SaveRecord()
    {
        float? previousBest = recordStorage.Load(settings.Id);
        if (previousBest == null || model.TimeLeft > previousBest.Value)
        {
            recordStorage.Save(settings.Id, model.TimeLeft);
        }
    }

    private void GoToMenu()
    {
        sceneLoader.LoadMenu();
    }

    private void Restart()
    {
        StartGame();
    }
}