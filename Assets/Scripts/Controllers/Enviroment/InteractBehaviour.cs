using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public abstract class InteractBehaviour : MonoBehaviour, IEnviromentInteraction
{
    [Inject] protected GameplayController gameplayController;
    [SerializeField] private float _delay;
    [SerializeField] private bool _repeat;
    public string ActionName => "Interagir";

    public float Delay => _delay;

    public bool Repeat => _repeat;

    public abstract UniTask Execute(params object[] items);
}
