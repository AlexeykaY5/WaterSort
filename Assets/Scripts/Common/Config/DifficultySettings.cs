using UnityEngine;

[CreateAssetMenu(fileName = "DifficultySettings", menuName = "WaterSort/DifficultySettings")]
public class DifficultySettings : ScriptableObject
{
    [SerializeField] private string id = "";
    [SerializeField] private string difficultyName;
    [SerializeField] private int numberOfColors = 8;
    [SerializeField] private int tubeCapacity = 4;
    [SerializeField] private int numberOfEmptyTubes = 2;
    [SerializeField] private int timer = 90;
    [SerializeField] private int bonusTime = 30;

    public string Id => id;
    public string DifficultyName => difficultyName;
    public int ColorsCount => numberOfColors;
    public int TubeCapacity => tubeCapacity;
    public int EmptyTubeCount => numberOfEmptyTubes;
    public int FilledTubeCount => numberOfColors;
    public int StartTime => timer;
    public int BonusTime => bonusTime;
}