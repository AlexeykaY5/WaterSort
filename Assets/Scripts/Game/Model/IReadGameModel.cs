using System.Collections.Generic;

public interface IReadGameModel
{
    IReadOnlyList<IReadTubeModel> TubeStates { get; }
    int SelectedIndex { get; }
    float TimeLeft { get; }
    bool IsWon { get; }
    bool IsTimeUp { get; }
}