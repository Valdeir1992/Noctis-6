using Cysharp.Threading.Tasks;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

public class MapController : MonoBehaviour
{
    private MapMenuController _mapMenu;
    private RegionSO _currentRegion;
    [Inject(Id="MapMenu")] private AssetReference _mapMenuRef;

    private void Awake()
    {
        SetupInterface().Forget();
    }
    private void Start()
    {
        foreach(var info in FindObjectsByType<UIMapInfoController>(FindObjectsSortMode.None))
        {
            info.Show();
        }
    }
    private async UniTask SetupInterface()
    {
        var task = Addressables.InstantiateAsync(_mapMenuRef);
        task.Completed += ctx =>
        {
            _mapMenu = ctx.Result.GetComponent<MapMenuController>();
        };
        await task;
        await _mapMenu.Show();
        await _mapMenu.ShowRegion(_currentRegion);
    }
}
