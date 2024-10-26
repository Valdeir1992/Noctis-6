using System;
using System.Collections;
using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using Zenject;
using System.Threading.Tasks;

public class ScreenWarningController : MonoBehaviour
{
    private CancellationTokenSource _aliveToken;
    private ScreenWarningUIController _screenWarningUIController;
    [Inject] private ScreenWarningUIController.Factory _screenWarningUIFactory;

    private void Awake()
    {
        _aliveToken = new CancellationTokenSource();
        _screenWarningUIController = _screenWarningUIFactory.Create();
    }
    private void OnDestroy()
    {
        _aliveToken.Cancel();
    }
    public void ShowActions(IEnviromentInteraction action)
    {
        _screenWarningUIController.ShowActions(action);
    }

    public void HiddenActions()
    {
        _screenWarningUIController.HiddenActions();
    }
    public async UniTask CollectedItem(ItemSO item, int amount)
    {
        var ui = await Addressables.LoadAssetAsync<UIItemDataSO>(item.UIRef);
        var icon = await Addressables.LoadAssetAsync<Sprite>(ui.Icon);
        _screenWarningUIController.CollectedItem(icon, item.name, amount);
    }

    internal void ShowMagnifying()
    {
        //_screenWarningUIController.ShowMagnifying();
    }

    internal void HiddenMagnfying()
    {
        _screenWarningUIController.HiddenMagnifying();
    }

    internal Task Investigate(ItemBehaviour itemBehaviour)
    {
        throw new NotImplementedException();
    }

    public async UniTask ReadDocument(ItemSO item, DocumentType type)
    {
        await _screenWarningUIController.ShowText(item.Name,"Empty", type);
    }
}
