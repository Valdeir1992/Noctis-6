using Cysharp.Threading.Tasks;
using UnityEngine; 
using Zenject;

public class InvestigateBehaviour : MonoBehaviour, IEnviromentInteraction
{
    [Inject] private ScreenWarningController _screenWarningController;
    [SerializeField] private float _delay;
    [SerializeField] private bool _repeat;
    public string ActionName => "Investigar";

    public float Delay => _delay;

    public bool Repeat => _repeat;

    public async UniTask Execute(params object[] items)
    {
        await _screenWarningController.Investigate(((ItemBehaviour)items[0]));
    }
}