using UnityEngine; 
using Zenject;

public class GameplayControllerInstaller : MonoInstaller
{
    [SerializeField] private CharacterMediator _playerPref;
    [SerializeField] private PlayerSpawnController _playerSpawnPrefab;
    [SerializeField] private ScreenWarningController _screenWarningControllerPrefab;
    public override void InstallBindings()
    {
        Container.Bind<PlayerSpawnController>().FromComponentInNewPrefab(_playerSpawnPrefab).AsSingle();
        Container.BindFactory<CharacterMediator, CharacterMediator.Factory>().FromComponentInNewPrefab(_playerPref).AsSingle();
        Container.Bind<GameplayController>().FromComponentInHierarchy().AsCached();
        Container.Bind<ScreenWarningController>().FromComponentInNewPrefab(_screenWarningControllerPrefab).AsCached();
    }
}
