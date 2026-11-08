using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{

    [SerializeField] private ColorPalette colorPalette;     

    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<GameModel>().AsSingle();
        Container.Bind<ColorPalette>().FromInstance(colorPalette).AsSingle();
    }
}
