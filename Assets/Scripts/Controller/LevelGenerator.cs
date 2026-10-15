using System.Collections.Generic;

public static class LevelGenerator
{
    public static void Generate(List<Tube> tubes, DifficultySettings config, ColorPalette palette, System.Random random)
    {
        int totalTubes = config.FilledTubeCount + config.EmptyTubeCount;
        int tubeCapacity = config.TubeCapacity;

        if (tubes.Count == 0)
        {
            for (int i = 0; i < totalTubes; i++)
            {
                tubes.Add(new Tube(tubeCapacity));
            }
        }

        List<string> colors = new List<string>();
        foreach (string colorName in palette.GetNames(config.ColorsCount))
        {
            for (int j = 0; j < tubeCapacity; j++)
            {
                colors.Add(colorName);
            }
        }

        for (int i = colors.Count - 1; i > 0; i--)
        {
            int randomIndex = random.Next(0, i + 1);
            string temp = colors[i];
            colors[i] = colors[randomIndex];
            colors[randomIndex] = temp;
        }

        for (int i = 0; i < config.FilledTubeCount; i++)
        {
            tubes[i].Fill(colors.GetRange(i * tubeCapacity, tubeCapacity));
        }

        for (int i = config.FilledTubeCount; i < totalTubes; i++)
        {
            tubes[i].Clear();
        }
    }
}