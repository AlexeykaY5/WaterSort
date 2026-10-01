using System.Collections.Generic;

public static class LevelGenerator
{
    public static Tube[] Generate(LevelConfig config, System.Random random)
    {
        int totalTubes = config.FilledTubeCount + config.EmptyTubeCount;
        int tubeCapacity = config.TubeCapacity;


        Tube[] tubes = new Tube[totalTubes];
        List<int> colorIds = new List<int>();


        for(int i = 0; i < config.ColorsCount; i++)
        {
            for(int j = 0; j < tubeCapacity; j++)
            {
                colorIds.Add(i);
            }
        }

        for(int i = colorIds.Count -1; i > 0; i--)
        {
            int randomIndex = random.Next(0, i + 1);

            int temp = colorIds[i];
            colorIds[i] = colorIds[randomIndex];
            colorIds[randomIndex] = temp;
        }

        for(int i = 0; i < config.FilledTubeCount; i++)
        {
            List<int> tubeColors = colorIds.GetRange(i * tubeCapacity, tubeCapacity);
            tubes[i] = new Tube(tubeCapacity, tubeColors);
        }

        for (int i = 0; i < totalTubes - config.FilledTubeCount; i++)
        {
            tubes[config.FilledTubeCount + i] = new Tube(tubeCapacity);
        }

        return tubes;
    }
}