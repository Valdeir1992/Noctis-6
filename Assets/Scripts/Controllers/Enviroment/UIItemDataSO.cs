using UnityEngine;
using UnityEngine.AddressableAssets;

[System.Serializable,CreateAssetMenu(menuName = "Noctis/Items/UI")]
public class UIItemDataSO : ScriptableObject
{
    [SerializeField] private string _name;  
    [SerializeField] private ItemAttribute[] _attributes;
    [SerializeField] private AssetReferenceSprite _icon;

    public string Name { get => _name;}
    public ItemAttribute[] Attributes { get => _attributes;}
    public AssetReferenceSprite Icon { get => _icon;}
}
