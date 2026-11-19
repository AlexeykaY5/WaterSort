using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ConfigDatabase", menuName = "WaterSort/ConfigDatabase")]
public class ConfigDatabase : ScriptableObject
{
    [SerializeField] private List<LevelSettings> levels;

    public IReadOnlyList<LevelSettings> Levels => levels;

    public string GetLevelName(int index) => $"Level {index + 1}";

    public int IndexOf(LevelSettings level)
    {
        int index = levels.IndexOf(level);
        if(index == -1)
        {
            throw new KeyNotFoundException($"ConfigDatabase has no level '{level.name}'");
        }

        return index;
    }

    private void OnValidate()
    {
        if(levels == null)
        {
            return;
        }

        HashSet<string> ids = new HashSet<string>();

        foreach(LevelSettings level in levels)
        {
            if(level == null)
            {
                continue;
            }

            if (!ids.Add(level.Id))
            {
                Debug.LogError($"ConfigDatabase has two levels with id '{level.Id}'", this);
            }
        }
    }
}