using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ConfigDatabase", menuName = "WaterSort/ConfigDatabase")]
public class ConfigDatabase : ScriptableObject
{
    [SerializeField] private List<DifficultySettings> difficulties;
    [SerializeField] private List<DifficultySettings> levels;

    public IReadOnlyList<DifficultySettings> Difficulties => difficulties;

    public DifficultySettings GetDifficulty(string id)
    {
        DifficultySettings found = null;

        foreach (DifficultySettings difficulty in difficulties)
        {
            if (difficulty.Id != id)
            {
                continue;
            }

            if (found != null)
            {
                throw new InvalidOperationException($"ConfigDatabase has two difficulties with id '{id}'");
            }
            found = difficulty;
        }

        if (found == null)
        {
            throw new KeyNotFoundException($"ConfigDatabase has no difficulty with id '{id}'");
        }
        return found;
    }

}