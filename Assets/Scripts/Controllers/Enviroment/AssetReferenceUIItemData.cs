using UnityEditor;
using UnityEngine.AddressableAssets;

[System.Serializable]
public class AssetReferenceUIItemData : AssetReferenceT<UIItemDataSO>
{
    /// <summary>
    /// Constructs a new reference to a GameObject.
    /// </summary>
    /// <param name="guid">The object guid.</param>
    public AssetReferenceUIItemData(string guid) : base(guid)
    {
    }

    public override bool ValidateAsset(string path)
    {
#if UNITY_EDITOR
        var type = AssetDatabase.GetMainAssetTypeAtPath(path);
        return typeof(UIItemDataSO).IsAssignableFrom(type);
#else
            return false;
#endif
    }

#if UNITY_EDITOR
    public new UIItemDataSO editorAsset
    {
        get
        {
            if (CachedAsset != null || string.IsNullOrEmpty(AssetGUID))
                return CachedAsset as UIItemDataSO;

            var assetPath = AssetDatabase.GUIDToAssetPath(AssetGUID);
            var main = AssetDatabase.LoadMainAssetAtPath(assetPath) as UIItemDataSO;
            if (main != null)
                CachedAsset = main;
            return main;
        }
    }
#endif
}
