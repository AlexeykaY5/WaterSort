using UnityEngine;
using Zenject;

public class ProjectInstaller : MonoInstaller
{
    [SerializeField] private ConfigDatabase configDatabase;

    public override void InstallBindings()
    {
        Container.Bind<RecordStorage>().AsSingle();
        Container.Bind<SceneLoader>().AsSingle();

        Container.Bind<ConfigDatabase>().FromInstance(configDatabase).AsSingle();
    }
}