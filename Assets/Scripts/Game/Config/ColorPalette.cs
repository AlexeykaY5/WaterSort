using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ColorPalette", menuName = "WaterSort/ColorPalette")]
public class ColorPalette : ScriptableObject
{
    [Serializable]
    private struct Entry
    {
        public string name;
        public Color color;
    }

    [SerializeField] private Entry[] entries;

    private Dictionary<string, Color> colorsByName;

    private void OnValidate()
    {
        for (int i = 0; i < entries.Length; i++)
        {
            entries[i].color.a = 1f;
        }

        colorsByName = null;
    }

    public List<string> GetNames(int count)
    {
        BuildIfNeeded();

        if (count > entries.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(count), $"ColorPalette has only {entries.Length} colors, but {count} are needed");
        }

        List<string> names = new List<string>();
        for (int i = 0; i < count; i++)
        {
            names.Add(entries[i].name);
        }
        return names;
    }

    public Color GetColor(string colorName)
    {
        BuildIfNeeded();

        if (!colorsByName.TryGetValue(colorName, out Color color))
        {
            throw new KeyNotFoundException($"ColorPalette has no color named '{colorName}'");
        }
        return color;
    }

    private void BuildIfNeeded()
    {
        if (colorsByName != null)
        {
            return;
        }

        Dictionary<string, Color> result = new Dictionary<string, Color>();
        foreach (Entry entry in entries)
        {
            if (string.IsNullOrEmpty(entry.name))
            {
                throw new InvalidOperationException("ColorPalette has a color without a name");
            }

            if (result.ContainsKey(entry.name))
            {
                throw new InvalidOperationException($"ColorPalette has the color name '{entry.name}' twice");
            }

            result.Add(entry.name, entry.color);
        }

        colorsByName = result;
    }
}