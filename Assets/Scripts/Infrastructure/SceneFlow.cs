using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneFlow
{
    private const string MenuSceneName = "Menu";
    private const string GameSceneName = "Game";
    
    public static DifficultySettings SelectedDifficulty { get; private set; }

    public static void LoadGame(DifficultySettings difficulty)
    {
        SelectedDifficulty = difficulty;
        SceneManager.LoadScene(GameSceneName);
    }

    public static void LoadMenu()
    {
        SceneManager.LoadScene(MenuSceneName);
    }
}
