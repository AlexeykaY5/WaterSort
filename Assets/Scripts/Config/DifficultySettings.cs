using UnityEngine;


[CreateAssetMenu(fileName = "DifficultySettings", menuName = "WaterSort/DifficultySettings")]
public class DifficultySettings : ScriptableObject
{
    public string id = "";
    public int numberOfColors = 8;
    public int tubeCapacity = 4;
    public int numberOfEmptyTubes = 2;
    public int timer = 90;
    public int bonusTime = 30;
    public string difficultyName;

    public LevelConfig ToConfig()
    {
        return new LevelConfig(numberOfColors, tubeCapacity, 
                               numberOfEmptyTubes, timer, bonusTime);
    }
}