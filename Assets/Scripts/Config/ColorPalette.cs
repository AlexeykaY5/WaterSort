using UnityEngine;

[CreateAssetMenu(fileName = "ColorPalette", menuName = "WaterSort/ColorPalette")]
public class ColorPalette : ScriptableObject
{
    [System.Serializable]
    private struct Entry
    {
        public string name;
        public Color color;
    }

    [SerializeField] private Entry[] entries;

    public int Count => entries.Length;

    private void OnValidate()
    {
        for(int i = 0; i < entries.Length; i++)
        {
            entries[i].color.a = 1f;
        }
    }

    public Color GetColor(int colorId)
    {
        if (colorId < 0 || colorId >= entries.Length)
        {
            return Color.magenta;
        }
        return entries[colorId].color;
    }   
}
