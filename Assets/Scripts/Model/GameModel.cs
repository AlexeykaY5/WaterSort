using System.Collections.Generic;

public class GameModel : IGameState
{
    public List<Tube> Tubes { get; } = new List<Tube>();    
    public HashSet<string> CompletedColors { get; } = new HashSet<string>();
    public int SelectedIndex { get; set; } = -1;
    public float TimeLeft { get; set; }
    public bool IsWon { get; set; }
    public bool IsTimeUp { get; set; }
    public int Round { get; set; }

    public IReadOnlyList<ITubeState> TubeStates => Tubes;
    public bool IsOver => IsWon || IsTimeUp;
}