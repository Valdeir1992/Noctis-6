using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class UIMapInfoController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _infoName;
    [SerializeField] private Image _icon;
    [SerializeField] private RegionSO _region;

    public RegionSO Region { get => _region;}

    public async UniTask Show()
    {
        _infoName.text = _region.Name;
        if (_region.Icon.IsValid())
        {
            var icon = await Addressables.LoadAssetAsync<Sprite>(_region.Icon);
            _icon.sprite = icon;
        }
    }
}
