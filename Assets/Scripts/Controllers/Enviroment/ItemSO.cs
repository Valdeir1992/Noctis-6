using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(menuName ="Noctis/Items/Data")]
public class ItemSO : ScriptableObject
{
    [SerializeField] private string _name;
    [SerializeField] private AssetReference _icon;
    [SerializeField,TextArea(3,5)] private string _description;

    public string Name { get => _name;}
    public AssetReference Icon { get => _icon;}
    public string Description { get => _description;}
}
