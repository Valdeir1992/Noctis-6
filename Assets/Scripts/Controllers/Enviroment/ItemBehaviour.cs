using Cysharp.Threading.Tasks; 
using UnityEngine; 
using Zenject;

[RequireComponent(typeof(SphereCollider))]
public class ItemBehaviour : MonoBehaviour
{
    [Inject] private GameplayController _gameplayController;
    [SerializeField] private ItemSO _item;
    [SerializeField] private int _amount;

    public ItemSO Item { get => _item;}

    private void OnTriggerEnter(Collider other)
    {
        var action = GetComponent<IEnviromentInteraction>();
        _gameplayController.ShowActions(action);
        _gameplayController.SetAction(
            () => 
            {
                action.Execute(this,_amount);
                _gameplayController.HiddenActions();
                if (!action.Repeat)
                {
                    Destroy(gameObject);
                    _gameplayController.CleanAction();
                }
                else
                {
                    _gameplayController.CoolDownAction(action.Delay);
                }
            });
    }
    private void OnTriggerExit(Collider other)
    {
        _gameplayController.HiddenActions();
        _gameplayController.CleanAction();
    }
}
public enum VisibilityType
{
    VERY_FAR, 
    NEAR,
    FAR
}
public interface IEnviromentInteraction
{
    public string ActionName { get; }  
    public float Delay { get; }
    public bool Repeat { get; }
    public UniTask Execute(params object[] items);
}
