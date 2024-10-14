using Cysharp.Threading.Tasks;
using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UIElements;
using Zenject;

public class ScreenWarningUIController : MonoBehaviour
{
    private VisualElement _root;

    private void Awake()
    {
        _root = GetComponent<UIDocument>().rootVisualElement;
    }

    public void ShowActions(IEnviromentInteraction action)
    {
        var actionsUI = new EnviromentActionUI();
        actionsUI.Init(action);
        _root.Q<VisualElement>("Center").Add(actionsUI);
    }
    public void CollectedItem(Sprite icon, string name, int amount)
    {
        var item = new CollectedItemUI() { };
        item.Init(icon, name, amount);
        item.Move().Forget();
        _root.Q<VisualElement>("Right").Add(item);
    }

    public void HiddenActions()
    {
        var element = _root.Q<EnviromentActionUI>();
        if(element != null)
        {
            _root.Q<VisualElement>("Center").Remove(element);
        }
    }
    public class Factory : PlaceholderFactory<ScreenWarningUIController> { }

    public async UniTask ShowText(string name, string description, DocumentType type)
    {
        var document = _root.Q<VisualElement>("Document");
        var result = await Addressables.LoadAssetAsync<Sprite>(type.ToString());
        document.Q<VisualElement>("Background").style.backgroundImage = new StyleBackground(result);
        document.style.visibility = Visibility.Visible;

        var gameInput = new GameInputs();
        gameInput.Enable();
        var action = new GameInputs().Gameplay.Interact;

        await UniTask.WaitUntil(()=>action.WasPressedThisFrame());
    }

    internal void HiddenMagnifying()
    {
        _root.Q<VisualElement>("Target").style.visibility = Visibility.Hidden;
    }

    internal async void ShowMagnifying()
    {
        var target = _root.Q<VisualElement>("Target");
        var result = await Addressables.LoadAssetAsync<Sprite>("Magnifying");
        target.style.backgroundImage = new StyleBackground(result);
        target.style.visibility = Visibility.Visible;
    }
}
