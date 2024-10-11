using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.UIElements;

public class MapMenuController : MonoBehaviour
{
    private VisualElement _root;
    public async UniTask Show()
    {
        _root = GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("Root");
        _root.ToggleInClassList("visible");
        await UniTask.Delay(TimeSpan.FromSeconds(1));
    }

    public async UniTask ShowRegion(RegionSO currentRegion)
    {
        
    }
}
