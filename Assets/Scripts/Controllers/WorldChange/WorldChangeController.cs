using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

public class WorldChangeController : MonoBehaviour
{
    public Action<NoctisWorlds> OnChange;
    private float _elapsedTime = 0;
    private NoctisWorlds _currentChange = NoctisWorlds.COMMON;
    private CancellationTokenSource _worldChangeCancelToken;
    [SerializeField] private float _transitionTime;
    [SerializeField] private float _globalTransitionTime;

    public float GlobalTime { get => _globalTransitionTime; }
    public float TransitionTime { get => _transitionTime;}

    private void Update()
    {
        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            _currentChange = NoctisWorlds.NIGHTMARE;
        }
        if (Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            _currentChange = NoctisWorlds.DREAM;
        }
        if (Keyboard.current.digit3Key.wasPressedThisFrame)
        {
            _currentChange = NoctisWorlds.COMMON;
        }
    }

    public async UniTask Change()
    {
        if(_worldChangeCancelToken != null)
        {
            _worldChangeCancelToken.Cancel();
        }
        _worldChangeCancelToken = new CancellationTokenSource();
        try
        {
            while(_elapsedTime < _transitionTime) 
            {
                _elapsedTime += Time.deltaTime;
                await UniTask.NextFrame(cancellationToken:_worldChangeCancelToken.Token);
            }
            if (!_worldChangeCancelToken.IsCancellationRequested)
            {
                OnChange?.Invoke(_currentChange);
            }
        }
        catch(Exception ex)
        {
            while (_elapsedTime >= 0)
            {
                _elapsedTime -= Time.deltaTime;
                await UniTask.NextFrame();
            }
        }
        finally
        {
            _worldChangeCancelToken = null;
            _elapsedTime = 0;
        }
    }
    public void ChangeTo(NoctisWorlds world)
    {
        _currentChange = world;
    }
    public void Cancel()
    {
        if(_worldChangeCancelToken != null)
        {
            _worldChangeCancelToken.Cancel();
        }
    }
}
