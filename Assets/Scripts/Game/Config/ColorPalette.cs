using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ColorPalette", menuName = "WaterSort/ColorPalette")]
public class ColorPalette : ScriptableObject
{
    [Serializable]
    private struct NamedColor
    {
        public string name;
        public Color color;
    }

    [SerializeField] private NamedColor[] colors;

    private void OnValidate()
    {
        if(colors == null)
        {
            return;
        }

        HashSet<string> names = new HashSet<string>();

        for (int i = 0; i < colors.Length; i++)
        {
            colors[i].color.a = 1f;

            if (string.IsNullOrEmpty(colors[i].name))
            {
                Debug.LogError($"ColorPalette: Element {i} has no name", this);
            }
            else if (!names.Add(colors[i].name))
            {
                Debug.LogError($"ColorPalette has the color name '{colors[i].name}' twice");
            }

        }
    }

    public List<string> GetNames(int count)
    {
        if (count > colors.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(count), $"ColorPalette has only {colors.Length} colors, but {count} are needed");
        }

        List<string> names = new List<string>();
        for (int i = 0; i < count; i++)
        {
            names.Add(colors[i].name);
        }
        return names;
    }

    public Color GetColor(string colorName)
    {   
        foreach(NamedColor namedColor in colors)
        {
            if(namedColor.name == colorName)
            {
                return namedColor.color;
            }
        }
        throw new KeyNotFoundException($"ColorPalette has no color named '{colorName}'");
    }
}