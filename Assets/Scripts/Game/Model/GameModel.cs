using System.Collections.Generic;

public class GameModel : IReadGameModel
{
    public List<TubeModel> Tubes { get; } = new List<TubeModel>();    
    public HashSet<string> CompletedColors { get; } = new HashSet<string>();
    public int SelectedIndex { get; set; } = -1;
    public float TimeLeft { get; set; }
    public bool IsWon { get; set; }
    public bool IsTimeUp { get; set; }
    public IReadOnlyList<IReadTubeModel> TubeStates => Tubes;
}