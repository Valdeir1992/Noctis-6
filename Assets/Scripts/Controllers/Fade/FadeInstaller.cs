using UnityEngine;
using Zenject;

public class FadeInstaller : MonoInstaller
{
    [SerializeField] private GameObject _fadeControllerPrefab;
    public override void InstallBindings()
    {
        Container.Bind<IFadeController>().FromComponentInNewPrefab(_fadeControllerPrefab).AsCached();
    }
}
