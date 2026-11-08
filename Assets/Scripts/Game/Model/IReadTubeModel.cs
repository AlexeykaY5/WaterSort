using System.Collections.Generic;

public interface IReadTubeModel
{
    IReadOnlyList<string> Contents { get; }
    int Capacity { get; }
}