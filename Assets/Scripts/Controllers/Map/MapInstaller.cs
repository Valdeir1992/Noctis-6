using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

public class MapInstaller : MonoInstaller
{
    [SerializeField] private AssetReference _mapMenuRef;
    public override void InstallBindings()
    {
        Container.Bind<AssetReference>().WithId("MapMenu").FromInstance(_mapMenuRef).AsCached();
    }
}