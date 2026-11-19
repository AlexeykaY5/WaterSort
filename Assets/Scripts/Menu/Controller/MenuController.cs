using UnityEngine;
using Zenject;

public class MenuController : MonoBehaviour
{
    private ConfigDatabase configs;
    private SceneLoader sceneLoader;
    private RecordStorage recordStorage;

    public int LevelCount => configs.Levels.Count;
    public string GetLevelName(int index) => configs.GetLevelName(index);

    public float? GetRecord(int index) => recordStorage.Load(configs.Levels[index].Id);
    
    [Inject]
    public void Construct(ConfigDatabase configs, SceneLoader sceneLoader, RecordStorage recordStorage)
    {
        this.configs = configs;
        this.sceneLoader = sceneLoader;
        this.recordStorage = recordStorage;
    }

    public void Play(int index)
    {
        sceneLoader.LoadGame(configs.Levels[index]);
    }
}