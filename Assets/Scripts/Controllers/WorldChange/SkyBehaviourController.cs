using Cysharp.Threading.Tasks;
using System;
using System.Linq;
using UnityEngine;
using Zenject;

public class SkyBehaviourController : MonoBehaviour
{
    [SerializeField] private Material _skyMaterial;
    [Inject] private WorldChangeController _worldChange;
    [SerializeField] private SkyChange[] _skyChange;

    private void OnEnable()
    {
        _worldChange.OnChange += Change;
    }

    private void OnDisable()
    {
        _worldChange.OnChange -= Change;
    }

    private async void Change(NoctisWorlds current)
    {
        var change = _skyChange.First(x => x.World == current);
        await SkyWorldChange(change);
    }
    private async UniTask SkyWorldChange(SkyChange change)
    {
        var time =(change.Global)? _worldChange.GlobalTime:change.Time;
        var sky = _skyMaterial.GetColor("_SkyTint");
        var ground = _skyMaterial.GetColor("_GroundColor");
        for(float elapsedTime = 0;elapsedTime <1.1f;elapsedTime += Time.deltaTime/time)
        {
            _skyMaterial.SetColor("_SkyTint", Color.Lerp(sky, change.Sky, elapsedTime));
            _skyMaterial.SetColor("_GroundColor", Color.Lerp(ground, change.Ground, elapsedTime));
            await UniTask.NextFrame();
        }
    }
}
[System.Serializable]
public class SkyChange
{
    public NoctisWorlds World;
    public Color Sky;
    public Color Ground;
    public float Time;
    public bool Global;
}