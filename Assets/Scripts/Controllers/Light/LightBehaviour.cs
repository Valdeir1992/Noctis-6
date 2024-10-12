using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(Light))]
public class LightBehaviour : MonoBehaviour
{
    private CancellationTokenSource _aliveCancelToken;
    private Light _currentLigth;
    [Inject] private WorldChangeController _worldChangeController;
    [SerializeField] private float _delay;
    [SerializeField] private float _time;
    [SerializeField] private AnimationCurve _curve;

    private void Awake()
    {
        _aliveCancelToken = new CancellationTokenSource();
        _currentLigth = GetComponent<Light>();
    }
    private void Start()
    {
        Behaviour();
    }
    private void OnDestroy()
    {
        _aliveCancelToken.Cancel();
    }
    private async UniTask Behaviour()
    {
        float startValue = _currentLigth.intensity;
        while (true)
        {
            float elapsedTime = 0;
            while (elapsedTime < _time && !_worldChangeController.Transition)
            {
                elapsedTime += Time.deltaTime;
                _currentLigth.intensity = _curve.Evaluate(elapsedTime / _time) * startValue;
                await UniTask.NextFrame(cancellationToken: _aliveCancelToken.Token);
            }
            await UniTask.Delay(TimeSpan.FromSeconds(_delay),cancellationToken:_aliveCancelToken.Token);
        }

    }
}
