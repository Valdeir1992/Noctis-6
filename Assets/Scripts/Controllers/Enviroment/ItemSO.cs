using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(menuName ="Noctis/Items/Data")]
public class ItemSO : ScriptableObject
{
    [SerializeField] private string _name; 
    [SerializeField] private AssetReferenceUIItemData _uiRef; 

    public string Name { get => _name;} 
    public AssetReferenceUIItemData UIRef { get => _uiRef;}
}
