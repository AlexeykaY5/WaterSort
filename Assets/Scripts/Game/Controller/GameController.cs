using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

public class GameController : MonoBehaviour
{
    public event Action LevelStarted;
    public event Action<int, int> SelectionChanged;
    public event Action<int, int> Poured;
    public event Action<int> TimeChanged;
    public event Action GameWon;
    public event Action GameLost;

    public IReadGameModel Model => model;
    public string LevelName => settings.DifficultyName;

    [SerializeField] private GameHudView gameHudView;

    private GameModel model;
    private RecordStorage recordStorage;
    private ConfigDatabase configs;
    private LevelSelection levelSelection;
    private SceneLoader sceneLoader;
    private ColorPalette palette;

    private DifficultySettings settings;
    private readonly System.Random random = new System.Random();
    private readonly HashSet<int> busyTubes = new HashSet<int>();
    private int shownSeconds = -1;

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
        gameHudView.MenuClicked += GoToMenu;
    }

    private void Start()
    {
        settings = configs.GetDifficulty(levelSelection.SelectedId);
        StartGame();
    }

    private void Update()
    {
        if (model.IsWon || model.IsTimeUp)
        {
            return;
        }

        model.TimeLeft -= Time.deltaTime;

        if (model.TimeLeft <= 0)
        {
            model.TimeLeft = 0f;
            model.IsTimeUp = true;
            EndGame();
        }
        
        int seconds = Mathf.CeilToInt(model.TimeLeft);
        if(seconds == shownSeconds)
        {
            return;
        }

        shownSeconds = seconds;
        TimeChanged?.Invoke(shownSeconds);
    }

    public void ClickTube(int index)
    {
        if (model.IsWon || model.IsTimeUp)
        {
            return;
        }

        if (busyTubes.Contains(index))
        {
            return;
        }

        if (model.SelectedIndex == -1)
        {
            if (IsEmpty(model.Tubes[index]))
            {
                return;
            }
            Select(index);
        }
        else if (model.SelectedIndex == index)
        {
            Select(-1);
        }
        else
        {
            int from = model.SelectedIndex;
            Select(-1);
            Pour(from, index);
        }

        if (model.IsWon)
        {
            SaveRecord();
            EndGame();
        }
    }

    public void PourFinished(int fromIndex, int toIndex)
    {
        busyTubes.Remove(fromIndex);
        busyTubes.Remove(toIndex);

        if (model.IsTimeUp || model.IsWon)
        {
            EndGame();
        }
    }

    private bool IsEmpty(TubeModel tube) => tube.Colors.Count == 0;

    private bool IsFull(TubeModel tube) => tube.Colors.Count >= tube.Capacity;

    private int FreeSpace(TubeModel tube) => tube.Capacity - tube.Colors.Count;

    private void StartGame()
    {
        Generate();
        model.SelectedIndex = -1;
        model.TimeLeft = settings.StartTime;
        shownSeconds = -1;
        model.IsWon = false;
        model.IsTimeUp = false;
        model.CompletedColors.Clear();
        busyTubes.Clear();
        LevelStarted?.Invoke();
    }

    private void Generate()
    {
        int totalTubes = settings.FilledTubeCount + settings.EmptyTubeCount;
        int tubeCapacity = settings.TubeCapacity;

        if (model.Tubes.Count == 0)
        {
            for (int i = 0; i < totalTubes; i++)
            {
                model.Tubes.Add(new TubeModel(tubeCapacity));
            }
        }

        List<string> colors = new List<string>();
        foreach (string colorName in palette.GetNames(settings.ColorsCount))
        {
            for (int j = 0; j < tubeCapacity; j++)
            {
                colors.Add(colorName);
            }
        }

        for (int i = colors.Count - 1; i > 0; i--)
        {
            int randomIndex = random.Next(0, i + 1);
            string temp = colors[i];
            colors[i] = colors[randomIndex];
            colors[randomIndex] = temp;
        }

        for (int i = 0; i < settings.FilledTubeCount; i++)
        {
            model.Tubes[i].Colors.Clear();
            model.Tubes[i].Colors.AddRange(colors.GetRange(i * tubeCapacity, tubeCapacity));
        }

        for (int i = settings.FilledTubeCount; i < totalTubes; i++)
        {
            model.Tubes[i].Colors.Clear();
        }
    }

    private void Select(int index)
    {
        int previous = model.SelectedIndex;
        model.SelectedIndex = index;
        SelectionChanged?.Invoke(previous, model.SelectedIndex);
    }

    private void Pour(int fromIndex, int toIndex)
    {
        int poured = PourInto(model.Tubes[fromIndex], model.Tubes[toIndex]);
        if (poured == 0)
        {
            return;
        }

        busyTubes.Add(fromIndex);
        busyTubes.Add(toIndex);

        Poured?.Invoke(fromIndex, toIndex);

        model.IsWon = CheckWin();

        if (!model.IsWon)
        {
            TryGiveBonus(toIndex);
        }
    }

    private string TopColor(TubeModel tube)
    {
        if (IsEmpty(tube))
        {
            return null;
        }
        else
        {
            return tube.Colors[tube.Colors.Count - 1];
        }
    }

    private int PourInto(TubeModel from, TubeModel to)
    {
        if (!CanPourInto(from, to))
        {
            return 0;
        }

        int result = Mathf.Min(TopGroupSize(from), FreeSpace(to));
        string topColor = TopColor(from);

        for (int i = 0; i < result; i++)
        {
            from.Colors.RemoveAt(from.Colors.Count - 1);
            to.Colors.Add(topColor);
        }
        return result;
    }

    private bool IsSingleColor(TubeModel tube)
    {
        if (!IsFull(tube))
        {
            return false;
        }

        foreach (string content in tube.Colors)
        {
            if (content != tube.Colors[0])
            {
                return false;
            }
        }
        return true;
    }

    private bool CanPourInto(TubeModel from, TubeModel to)
    {
        if (IsEmpty(from))
        {
            return false;
        }
        else if (IsFull(to))
        {
            return false;
        }
        else if (from == to)
        {
            return false;
        }
        return true;
    }

    private int TopGroupSize(TubeModel tube)
    {
        if (IsEmpty(tube))
        {
            return 0;
        }
        int count = 1;
        string top = tube.Colors[tube.Colors.Count - 1];

        for (int i = tube.Colors.Count - 2; i >= 0; i--)
        {
            if (tube.Colors[i] == top)
            {
                count++;
            }
            else
            {
                break;
            }
        }
        return count;
    }

        private void TryGiveBonus(int index)
    {
        TubeModel tube = model.Tubes[index];
        if (!IsSingleColor(tube))
        {
            return;
        }

        string color = TopColor(tube);
        if (model.CompletedColors.Add(color))
        {
            model.TimeLeft += settings.BonusTime;
        }
    }

    private void SaveRecord()
    {
        float? previousBest = recordStorage.Load(settings.Id);
        if (previousBest == null || model.TimeLeft > previousBest.Value)
        {
            recordStorage.Save(settings.Id, model.TimeLeft);
        }
    }

    private bool CheckWin()
    {
        foreach (TubeModel tube in model.Tubes)
        {
            if (!IsSingleColor(tube) && !IsEmpty(tube))
            {
                return false;
            }
        }
        return true;
    }

    private void EndGame()
    {
        if(busyTubes.Count > 0)
        {
            return;
        }
        if (model.IsWon)
        {
            GameWon?.Invoke();
        }
        else
        {
            GameLost?.Invoke();
        }
    }
    public void GoToMenu()
    {
        sceneLoader.LoadMenu();
    }

    public void Restart()
    {
        StartGame();
    }
}