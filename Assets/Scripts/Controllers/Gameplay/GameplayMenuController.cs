using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UIElements;

public class GameplayMenuController : MonoBehaviour
{
    private VisualElement _root; 
    public async UniTask Show(CancellationToken token)
    {
        try
        {
            _root = GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("Root");
            _root.ToggleInClassList("visible");
        }
        catch
        {
            
        }
    }
    public async UniTask Hidden(CancellationToken token)
    {
        try
        { 
            _root.ToggleInClassList("visible");
            await UniTask.Delay(TimeSpan.FromSeconds(1f));
        }
        catch
        {
            _root.ToggleInClassList("visible");
        }
        finally
        {
            Addressables.ReleaseInstance(gameObject);
        }
    }
}
