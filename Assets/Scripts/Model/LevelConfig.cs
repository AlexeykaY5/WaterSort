public class LevelConfig
{
    public int ColorsCount { get; }
    public int TubeCapacity { get; }
    public int EmptyTubeCount { get; }
    public int StartTime { get; }
    public int BonusTime { get; }
    public int FilledTubeCount => ColorsCount;
    public LevelConfig(int colorsCount, int tubeCapacity, 
                       int emptyTubeCount, int startTime, int bonusTime)
    {
        ColorsCount = colorsCount;
        TubeCapacity = tubeCapacity;
        EmptyTubeCount = emptyTubeCount;
        StartTime = startTime;
        BonusTime = bonusTime;
    }
}
