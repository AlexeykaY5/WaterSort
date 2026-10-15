using System.Collections.Generic;

public interface ITubeState
{
    IReadOnlyList<string> Contents { get; }
    int Capacity { get; }
}