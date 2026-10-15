using UnityEngine;
using Zenject;

public class MenuController : MonoBehaviour
{
    [SerializeField] private MenuView menuView;

    private ConfigDatabase configs;
    private LevelSelection levelSelection;
    private SceneLoader sceneLoader;

    [Inject]
    public void Construct(ConfigDatabase configs, LevelSelection levelSelection, SceneLoader sceneLoader)
    {
        this.configs = configs;
        this.levelSelection = levelSelection;
        this.sceneLoader = sceneLoader;
    }

    private void Awake()
    {
        menuView.PlayClicked += OnPlayClicked;
    }

    private void OnPlayClicked(int index)
    {
        levelSelection.Select(configs.Difficulties[index].Id);
        sceneLoader.LoadGame();
    }
}