using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class CollectBehaviour : MonoBehaviour, IEnviromentInteraction
{ 
    [Inject] private ScreenWarningController _screenWarningController;
    public string ActionName => "Coletar";

    public float Delay => 0;

    public bool Repeat => false;

    public async UniTask Execute(params object[] items)
    {
        await _screenWarningController.CollectedItem(((ItemBehaviour)items[0]).Item, (int)items[1]);
    }
}
