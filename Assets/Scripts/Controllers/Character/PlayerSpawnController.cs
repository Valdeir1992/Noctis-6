using System.Linq;
using UnityEngine; 
using Zenject;

public class PlayerSpawnController : MonoBehaviour
{
    private CharacterMediator _player;
    [SerializeField] private bool _spawnOnStart;
    [SerializeField] private bool _spawnOnSpawnPoint;
    [Inject] private CharacterMediator.Factory _playerFactory;

    public CharacterMediator Player { get => _player;}

    private void Start()
    {
        if (_spawnOnStart)
        {
            _player = _playerFactory.Create(); 
        }
        else
        {
            _player = FindAnyObjectByType<CharacterMediator>();
        }
        if (_spawnOnSpawnPoint)
        {
            _player.transform.position = FindObjectsByType<SpawnPoint>(FindObjectsSortMode.None).First(x=>x.Name == "Start").transform.position;
        }
    }
}
