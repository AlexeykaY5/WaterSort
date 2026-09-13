using UnityEngine;


[CreateAssetMenu(fileName = "DifficultySettings", menuName = "WaterSort/DifficultySettings")]
public class DifficultySettings : ScriptableObject
{
    public int numberOfColors = 8;
    public int numberOfColorsInTube = 4;
    public int numberOfFilledTubes = 3;
    public int numberOfEmptyTubes = 2;
    public int timer = 90;
    public int bonusTime = 30;
    public string difficultyName;
}