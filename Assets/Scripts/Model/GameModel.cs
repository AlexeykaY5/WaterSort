using System.Collections.Generic;

public class GameModel
{
    private readonly Tube[] tubes;
    private readonly LevelConfig config;
    private readonly HashSet<int> completedColors = new HashSet<int>();
    private int selectedIndex = -1;

    public GameModel(Tube[] tubes, LevelConfig config)
    {
        this.tubes = tubes;
        this.config = config;
        TimeLeft = config.StartTime;
    }

    public int TubeCount => tubes.Length;
    public int SelectedIndex => selectedIndex;
    public float TimeLeft { get; private set; }
    public bool IsWon { get; private set; }
    public bool IsTimeUp { get; private set; }
    public bool IsOver => IsWon || IsTimeUp;


    public bool IsNewRecord(float? previousBest)
    {
        if(previousBest == null)
        {
            return true;
        }
        else
        {
            return TimeLeft > previousBest.Value;
        }
    }

    public IReadOnlyList<int> GetTubeColors(int index)
    {
        return tubes[index].GetContents();
    }

    public MoveResult Click(int index)
    {
        if (IsOver)
        {
            return MoveResult.None;
        }

        if(selectedIndex == -1)
        {
            if (tubes[index].IsEmpty)
            {
                return MoveResult.None;
            }

            selectedIndex = index;
            return MoveResult.Selected;
        }
        else if (selectedIndex == index)
        {
            selectedIndex = -1;
            return MoveResult.Deselected;
        }

        tubes[selectedIndex].PourInto(tubes[index]);
        selectedIndex = -1;

        TryGiveBonus(index);
        IsWon = CheckWin();

        return MoveResult.Poured;
    }

    public void Tick(float deltaTime)
    {
        if (IsOver)
        {
            return;
        }
        
        TimeLeft -= deltaTime;

        if(TimeLeft <= 0f)
        {
            TimeLeft = 0f;
            IsTimeUp = true;
        }
    }

    private void TryGiveBonus(int index)
    {
        if (!tubes[index].IsSingleColor())
        {
            return;
        }

        int color = tubes[index].TopColor().Value;

        if (!CheckWin())
        {
            if (completedColors.Add(color))
            {
                TimeLeft += config.BonusTime;
            }
        }
    }

    private bool CheckWin()
    {
        for (int i = 0; i < tubes.Length; i++)
        {
            if (!tubes[i].IsSingleColor() && !tubes[i].IsEmpty)
            {
                return false;
            }
        }
        return true;
    }
}