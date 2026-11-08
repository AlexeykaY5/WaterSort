using System.Collections.Generic;

public class TubeModel : IReadTubeModel
{

    public List<string> Colors { get; } = new List<string>();
    public IReadOnlyList<string> Contents => Colors;
    public int Capacity { get; }
    public TubeModel(int capacity)
    {
        Capacity = capacity;
    }
}