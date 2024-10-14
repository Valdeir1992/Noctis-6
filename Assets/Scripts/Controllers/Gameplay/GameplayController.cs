using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;
using Zenject;

public class GameplayController : MonoBehaviour
{
    public Action OnPause;
    public Action OnResume;
    public Action OnInteract;
    private bool _isPaused;
    private bool _mapVisible;
    private bool _canWorldChange = true;
    private bool _canInteract = true;
    private Action _currentAction;
    private GameInputs _inputs;
    private GameplayMenuController _gameplayMenu;
    private CancellationTokenSource _menuCancelToken;
    private CancellationTokenSource _mapCancelToken;
    private CancellationTokenSource _aliveCancelToken;
    [Inject] private ScreenWarningController _screenWarning;
    [Inject] private PlayerSpawnController _playerSpawn;
    [Inject] private WorldChangeController _worldChange;
    [Inject] private IFadeController _fadeController;
    [Inject(Id = "GameplayMenu")] private AssetReference _gameplayMenuRef;

    private void Start()
    {
        SetupInputs();
        _aliveCancelToken = new CancellationTokenSource();
    }

    internal async UniTaskVoid CoolDownAction(float delay)
    {
        _canInteract = false;
        await UniTask.Delay(TimeSpan.FromSeconds(delay),cancellationToken:_aliveCancelToken.Token);
        _canInteract = true;
    }
    public async void ReadDocument(ItemSO item,DocumentType type)
    {
        _playerSpawn.Player.ToggleMove(false);
        _canInteract = false;
        await _screenWarning.ReadDocument(item,type);
        _canInteract = true;
        _playerSpawn.Player.ToggleMove(true);
    }
    internal void SetAction(Action action)
    {
        _currentAction = action;
    }
    public void CleanAction()
    {
        _currentAction = null; ;
    }

    internal void ShowActions(IEnviromentInteraction enviromentInteraction)
    {
        _screenWarning.ShowActions(enviromentInteraction);
    }

    internal void HiddenActions()
    {
        _screenWarning.HiddenActions();
    }

    private void SetupInputs()
    {
        _inputs = new GameInputs();
        _inputs.Gameplay.Pause.started += Pause;
        _inputs.Gameplay.Map.started += Map;
        _inputs.Gameplay.WorldChange.started += WorldChange;
        _inputs.Gameplay.WorldChange.canceled += WorldChangeCancel;
        _inputs.Gameplay.Interact.started += ctx =>
        {
            _currentAction?.Invoke();  
        };
        _inputs.Gameplay.Enable();
    }

    private async void WorldChange(InputAction.CallbackContext ctx)
    {
        if (!_canWorldChange)
            return;
        await _worldChange.Change();
    }
    private void WorldChangeCancel(InputAction.CallbackContext ctx)
    {
        _worldChange.Cancel();
    }

    private async void Map(InputAction.CallbackContext ctx)
    {
        _mapVisible = !_mapVisible;
        if (_mapVisible)
        {
            await OpenMap();
        }
        else
        {
            await CloseMap();
        }
    }
    private async UniTask OpenMap()
    {
        try
        {
            if (_mapCancelToken != null)
            {
                _mapCancelToken.Cancel();
            }
            _mapCancelToken = new CancellationTokenSource();
            await _fadeController.FadeOut();
            var load = Addressables.LoadSceneAsync("Map", UnityEngine.SceneManagement.LoadSceneMode.Additive);
            var instance = await load;
            await instance.ActivateAsync();
            await _fadeController.FadeIn();
        }
        catch (Exception ex)
        {

        }
        finally
        {
            _mapCancelToken = null;
        }
    }
    private async UniTask CloseMap()
    {
        try
        {
            if (_mapCancelToken != null)
            {
                _mapCancelToken.Cancel();
            }
            _mapCancelToken = new CancellationTokenSource(); 
            _mapCancelToken = new CancellationTokenSource();
            await _fadeController.FadeOut();
            var task = UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync("Map");
            await task;
            await _fadeController.FadeIn();

        }
        catch (Exception ex)
        {

        }
        finally
        {
            _mapCancelToken = null;
        }
    }
    private async UniTask ShowMenu()
    {
        try
        {
            if(_menuCancelToken != null)
            {
                _menuCancelToken.Cancel();
            }
            _menuCancelToken = new CancellationTokenSource();

            var instanceMenu = Addressables.InstantiateAsync(_gameplayMenuRef, transform);
            instanceMenu.Completed += ctx =>
            {
                _gameplayMenu = ctx.Result.GetComponent<GameplayMenuController>();
            }; 
            await instanceMenu;
            await UniTask.Delay(TimeSpan.FromSeconds(0.3f));
            await _gameplayMenu.Show(_menuCancelToken.Token);
        }
        catch (Exception ex)
        {
            
        }
        finally
        {
            _menuCancelToken = null;
        }
    }
    private async UniTask HiddenMenu()
    {
        try
        {
            if (_gameplayMenu == null)
                return;

            if (_menuCancelToken != null)
            {
                _menuCancelToken.Cancel();
            }
            _menuCancelToken = new CancellationTokenSource();
            await _gameplayMenu.Hidden(_aliveCancelToken.Token);
            Addressables.ReleaseInstance(_gameplayMenu.gameObject);
            _gameplayMenu = null;
        }
        catch(Exception ex)
        {
            
        }
        finally
        {
            _menuCancelToken = null;
        }
    }
    private async void Pause(InputAction.CallbackContext ctx)
    {
        _isPaused = !_isPaused;
        if (_isPaused)
        {
            OnPause?.Invoke();
            await ShowMenu();
        }
        else
        {
            OnResume?.Invoke();
            await HiddenMenu();
        }
    }
}

public enum DocumentType
{
    LETTER,
    BOOK,
}
