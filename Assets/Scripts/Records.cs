using UnityEngine;

public static class Records
{
    private static string Key(string difficultyName)
    {
        return "record_" + difficultyName;
    }

    public static bool HasRecord(string difficultyName)
    {
        return PlayerPrefs.HasKey(Key(difficultyName));
    }

    public static float GetRecord(string difficultyName)
    {
        return PlayerPrefs.GetFloat(Key(difficultyName));
    }

    public static void TrySetRecord(string difficultyName, float timeLeft)
    {
        if (!HasRecord(difficultyName) || timeLeft > GetRecord(difficultyName))
        {
            PlayerPrefs.SetFloat(Key(difficultyName), timeLeft);
            PlayerPrefs.Save();
        }
    }
}