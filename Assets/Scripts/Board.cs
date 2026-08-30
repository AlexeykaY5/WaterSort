using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Board : MonoBehaviour
{
    private DifficultySettings settings;
    public TextMeshProUGUI difficultyText;
    public GameTimer gameTimer;
    public TubeView tubeViewPrefab;
    public Transform tubeContainer;
    public ScreenManager screenManager;
    private HashSet<WaterColor> visitedColors = new HashSet<WaterColor>();
    private Tube[] tubes;
    private TubeView[] tubeViews;
    private int selectedIndex = -1;

    public void StartGame(DifficultySettings settings)
    {
        selectedIndex = -1;
        visitedColors.Clear();
        if(tubeViews != null)
        {
            foreach (var tube in tubeViews)
            {
                Destroy(tube.gameObject);
            }
        }
        
        this.settings = settings;

        this.difficultyText.text = settings.difficultyName;

        gameTimer.StartTimer(settings.timer);

        tubes = LevelGenerator.Generate(settings);

        tubeViews = new TubeView[tubes.Length];

        for (int i = 0; i < tubes.Length; i++)
        {
            TubeView view = Instantiate(tubeViewPrefab, tubeContainer);

            view.Init(i, this);

            tubeViews[i] = view;
            view.Render(tubes[i]);

        }
        screenManager.ShowGamePanel();
    }

    public void OnTubeClicked(int tubeIndex)
    {
        if (IsGameOver())
        {
            return;
        }

        if (selectedIndex == -1)
        {
            if(!tubes[tubeIndex].IsEmpty())
            {
                tubeViews[tubeIndex].SetSelected(true);
                selectedIndex = tubeIndex;
            }
        }
        else if (selectedIndex == tubeIndex)
        {
            tubeViews[selectedIndex].SetSelected(false);
            selectedIndex = -1;
        }
        else
        {
            tubeViews[selectedIndex].SetSelected(false);
            tubes[selectedIndex].PourInto(tubes[tubeIndex]);
            tubeViews[selectedIndex].Render(tubes[selectedIndex]);
            tubeViews[tubeIndex].Render(tubes[tubeIndex]);

            if(tubes[tubeIndex].IsSingleColor())
            {
                if (!visitedColors.Contains(tubes[tubeIndex].TopColor().Value))
                {
                    visitedColors.Add(tubes[tubeIndex].TopColor().Value);
                    gameTimer.AddBonusTime(settings.bonusTime);
                }
            }
            if (CheckWin())
            {
                Win();
                return;
            }
            selectedIndex = -1;
        }
    }

    private bool CheckWin()
    {
        for(int i = 0; i < tubes.Length; i++)
        {
            if (!tubes[i].IsSingleColor() && !tubes[i].IsEmpty())
            {
                return false;
            }
        }
        return true;
    }

    private bool IsGameOver()
    {
        if(gameTimer.isTimePassed)
        {
            return true;
        }else if (CheckWin())
        {
            return true;
        }
        return false;
    }

    private void Win()
    {
        gameTimer.StopTimer();

        Records.TrySetRecord(settings.difficultyName, gameTimer.TimeLeft);

        screenManager.ShowWinPanel();
    }

    private void Lose()
    {
        gameTimer.StopTimer();
        
        screenManager.ShowLosePanel();
    }

    public void GoToMenu()
    {
        gameTimer.StopTimer();
        screenManager.ShowMenuPanel();
    }

    public void Restart()
    {
        StartGame(settings);
    }

    private void Start()
    {
        gameTimer.TimeUp += Lose;
    }

    private void OnDestroy()
    {
        gameTimer.TimeUp -= Lose;
    }
}