using UnityEngine;
using System.Collections.Generic;

public static class LevelGenerator
{
    public static Tube[] Generate(DifficultySettings settings)
    {
        int totalTubes = settings.numberOfFilledTubes + settings.numberOfEmptyTubes;
        int numberOfColorsInTube = settings.numberOfColorsInTube;

        Tube[] tubes = new Tube[totalTubes];
        List<WaterColor> waterColors = new List<WaterColor>();

        for(int i = 0; i < settings.numberOfColors; i++)
        {
            for(int j = 0; j < numberOfColorsInTube; j++)
            {
                waterColors.Add((WaterColor)i);
            }
        }

        for(int i = waterColors.Count -1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);

            WaterColor temp = waterColors[i];
            waterColors[i] = waterColors[randomIndex];
            waterColors[randomIndex] = temp;
        }

        for(int i = 0; i < settings.numberOfFilledTubes; i++)
        {
            List<WaterColor> tubeColors = waterColors.GetRange(i * numberOfColorsInTube, numberOfColorsInTube);
            tubes[i] = new Tube(numberOfColorsInTube, tubeColors);
        }

        for (int i = 0; i < totalTubes - settings.numberOfFilledTubes; i++)
        {
            tubes[settings.numberOfFilledTubes + i] = new Tube(numberOfColorsInTube);
        }

        return tubes;
    }
}