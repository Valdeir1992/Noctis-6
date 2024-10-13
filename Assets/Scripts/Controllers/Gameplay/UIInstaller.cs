using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

public class UIInstaller : MonoInstaller
{
    [SerializeField] private AssetReference _gameplayMenuRef;
    [SerializeField] private ScreenWarningUIController _screenWarningPrefab;
    public override void InstallBindings()
    {
        Container.Bind<AssetReference>().WithId("GameplayMenu").FromInstance(_gameplayMenuRef).AsSingle();
        Container.BindFactory<ScreenWarningUIController, ScreenWarningUIController.Factory>().FromComponentInNewPrefab(_screenWarningPrefab).AsSingle();
    }
}
