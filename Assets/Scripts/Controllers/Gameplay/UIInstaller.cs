using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

public class UIInstaller : MonoInstaller
{
    [SerializeField] private AssetReference _gameplayMenuRef;
    public override void InstallBindings()
    {
        Container.Bind<AssetReference>().WithId("GameplayMenu").FromInstance(_gameplayMenuRef).AsSingle();
    }
}
