using Cysharp.Threading.Tasks;
using System.Linq;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(Light))]
public class LightBehaviourController : MonoBehaviour
{
    private Light _light;
    private LightChange _currentLightChange;
    [Inject] private WorldChangeController _worldChangeController;
    [SerializeField] private LightChange[] _changeLight;

    private void Awake()
    {
        _light = GetComponent<Light>();
        _currentLightChange = _changeLight[0];
    }
    private void OnEnable()
    {
        _worldChangeController.OnChange += Change;
    }
    private void OnDisable()
    {
        _worldChangeController.OnChange -= Change;
    }

    private async void Change(NoctisWorlds world)
    {
        var change = _changeLight.First(x => x.World == world);
        await UniTask.WhenAll(ChangeColor(_currentLightChange, change), ChangeIntensity(_currentLightChange, change));
        _currentLightChange = change;
    }
    private async UniTask ChangeColor(LightChange start, LightChange end)
    {
        float time = (end.Global) ? _worldChangeController.GlobalTime : end.TransitionTime;
        for(float elapsedTime  = 0;elapsedTime < 1.1f; elapsedTime += Time.deltaTime / time)
        {
            _light.color = Color.Lerp(start.Color, end.Color, elapsedTime);
            await UniTask.NextFrame();
        }
    }
    private async UniTask ChangeIntensity(LightChange start, LightChange end)
    {
        float time = (end.Global) ? _worldChangeController.GlobalTime : end.TransitionTime;
        for (float elapsedTime = 0; elapsedTime < 1.1f; elapsedTime += Time.deltaTime / time)
        {
            _light.intensity = Mathf.Lerp(start.Intensity, end.Intensity, elapsedTime);
            await UniTask.NextFrame();
        }
    }
}
[System.Serializable]
public class LightChange
{
    public Color Color;
    public float Intensity;
    public NoctisWorlds World;
    public bool Global;
    public float TransitionTime;
}
public enum NoctisWorlds
{
    NIGHTMARE,
    DREAM,
    COMMON
}
