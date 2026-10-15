using UnityEngine;
using Zenject;

public class ProjectInstaller : MonoInstaller
{
    [SerializeField] private ConfigDatabase configDatabase;
    [SerializeField] private ColorPalette colorPalette;

    public override void InstallBindings()
    {
        Container.Bind<RecordStorage>().AsSingle();
        Container.Bind<LevelSelection>().AsSingle();
        Container.Bind<SceneLoader>().AsSingle();

        Container.Bind<ConfigDatabase>().FromInstance(configDatabase).AsSingle();
        Container.Bind<ColorPalette>().FromInstance(colorPalette).AsSingle();
    }
}