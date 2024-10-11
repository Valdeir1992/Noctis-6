using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(menuName ="Noctis/Region/Data")]
public class RegionSO : ScriptableObject
{
    [SerializeField] private string _name;
    [SerializeField] private AssetReferenceSprite _icon;
    [SerializeField] private CollectibleRegion[] _collectibles;

    public string Name { get => _name;}
    public AssetReferenceSprite Icon { get => _icon;}
    public CollectibleRegion[] Collectibles { get => _collectibles;}
}
[System.Serializable]
public class CollectibleRegion
{
    public Collectible Collectible;
    public int Amount;
}
[System.Serializable]
public class Collectible
{
    public string Name;
    public CollectibleType Type;
} 
public enum CollectibleType
{
    BOOK,
    HERB,
    GEM,
    FRUIT,
    CHARM,
    LIQUID
}
