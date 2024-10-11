using UnityEngine;
using Zenject;

public class GameplayControllerInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<GameplayController>().FromComponentInHierarchy().AsCached();
    }
}
