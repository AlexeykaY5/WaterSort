using System.Collections.Generic;

public interface IGameState
{
    IReadOnlyList<ITubeState> TubeStates { get; }
    int SelectedIndex { get; }
    float TimeLeft { get; }
    bool IsWon { get; }
    bool IsTimeUp { get; }
    bool IsOver { get; }
    int Round { get; }
}