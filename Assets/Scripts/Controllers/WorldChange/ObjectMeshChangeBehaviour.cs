using Cysharp.Threading.Tasks; 
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

public class ObjectMeshChangeBehaviour : MonoBehaviour
{
    private MeshChange _currentChange;
    private BoxCollider _currentCollider;
    [Inject] private WorldChangeController _worldChangeController;
    [SerializeField] private MeshRenderer _render;
    [SerializeField] private MeshFilter _filter;
    [SerializeField] private MeshChange[] _meshChange;

    private void Awake()
    {
        _currentCollider = GetComponent<BoxCollider>();
    }

    private void Start()
    {
        _currentChange = _meshChange[0];
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
        var change = _meshChange.FirstOrDefault(x => x.World == world);
        if(change == default) 
        {
            _currentCollider.enabled = false;
            _filter.mesh = null;
        }
        else
        {
            _currentChange = change;
            _currentCollider.enabled = true;
            var taskMesh = Addressables.LoadAssetAsync<Mesh>(change.Mesh);
            taskMesh.Completed += ctx =>
            {
                _filter.mesh = ctx.Result;
            };
            var taskMaterial = Addressables.LoadAssetAsync<Material>(change.Material);
            taskMaterial.Completed += ctx =>
            {
                _render.materials[0] = ctx.Result;
            };
            await UniTask.WhenAll(taskMesh.ToUniTask(), taskMaterial.ToUniTask()); 
        }
    }
}
[System.Serializable]
public class MeshChange: Change
{ 
    public AssetReference Mesh;
    public AssetReference Material;
}
