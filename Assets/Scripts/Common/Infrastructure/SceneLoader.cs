using UnityEngine.SceneManagement;
using Zenject;

public class SceneLoader
{
    private const string MenuSceneName = "Menu";
    private const string GameSceneName = "Game";

    private readonly ZenjectSceneLoader zenjectSceneLoader;

    public SceneLoader(ZenjectSceneLoader zenjectSceneLoader)
    {
        this.zenjectSceneLoader = zenjectSceneLoader;
    }

    public void LoadMenu()
    {
        zenjectSceneLoader.LoadScene(MenuSceneName);
    }

    public void LoadGame(LevelSettings settings)
    {
        zenjectSceneLoader.LoadScene(GameSceneName, LoadSceneMode.Single,
                                     container => container.BindInstance(settings));
    }
}