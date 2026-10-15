using UnityEngine.SceneManagement;

public class SceneLoader
{
    private const string MenuSceneName = "Menu";
    private const string GameSceneName = "Game";

    public void LoadMenu()
    {
        SceneManager.LoadScene(MenuSceneName);
    }

    public void LoadGame()
    {
        SceneManager.LoadScene(GameSceneName);
    }
}