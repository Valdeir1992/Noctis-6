using Cysharp.Threading.Tasks;
using System; 
using System.Threading; 
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

public class EnviromentWarning : MonoBehaviour
{ 
    public Action OnNear;
    public Action OnFar;
    public Action OnVeryFar;
    private bool _iconTransition; 
    private VisibilityType _currentVisibility;
    private CancellationTokenSource _aliveCancelToken;
    [Inject] protected PlayerSpawnController _spawnController;
    [SerializeField] private TMPro.TextMeshProUGUI _textView;
    [SerializeField] private SpriteRenderer _spriteRender; 
    [SerializeField] private CollectibleType _type;
    [SerializeField] private string _name;
    [SerializeField] private VisibilitySO _visibility;

    private void Awake()
    {
        _aliveCancelToken = new CancellationTokenSource();
    }

    private void Start()
    {
        CheckVisibility().Forget();
        OnVeryFar += async ()=> 
        {
            _textView.SetText("");
            await TransitionIconTo("CHEST_ICON");
        };
        OnFar += async () =>
        {
            _textView.SetText(_name);
            await TransitionIconTo(_type.ToString());
        };
    }

    private async UniTask TransitionIconTo(string icon)
    {
        _iconTransition = true;
        await FadeOutIcon();
        _spriteRender.sprite = await Addressables.LoadAssetAsync<Sprite>(icon);
        await FadeInIcon();
        _iconTransition = false;
    }

    private async UniTask FadeOutIcon()
    {
        for(float elapsedTime = 0;elapsedTime < 1.1f;elapsedTime += Time.deltaTime/ 0.5f)
        {
            _spriteRender.color = Color.Lerp(new Color(1, 1, 1, 1), new Color(1, 1, 1, 0), elapsedTime);
            await UniTask.NextFrame(cancellationToken:_aliveCancelToken.Token);
        }
    }

    private async UniTask FadeInIcon()
    {
        for (float elapsedTime = 0; elapsedTime < 1.1f; elapsedTime += Time.deltaTime / 0.5f)
        {
            _spriteRender.color = Color.Lerp(new Color(1, 1, 1, 0), new Color(1, 1, 1, 1), elapsedTime);
            await UniTask.NextFrame(cancellationToken: _aliveCancelToken.Token);
        }
    }

    private void OnDestroy()
    {
        _aliveCancelToken.Cancel();
    }

    private async UniTaskVoid CheckVisibility()
    {
        float sqrDistance = 0;
        try
        {
            while (true)
            {
                var direction = (_spawnController.Player.transform.position - transform.position);
                sqrDistance = direction.sqrMagnitude;
                if(sqrDistance < _visibility.FarDistance * _visibility.FarDistance && sqrDistance > _visibility.NearDistance * _visibility.NearDistance)
                {
                    SetIconVisibility(VisibilityType.FAR); 
                }
                else if (sqrDistance < _visibility.NearDistance * _visibility.NearDistance)
                {
                    SetIconVisibility(VisibilityType.NEAR);
                }
                else
                {
                    SetIconVisibility(VisibilityType.VERY_FAR);
                }
                transform.LookAt(-direction,Vector3.up);
                await UniTask.Delay(TimeSpan.FromSeconds(0.1f),cancellationToken:_aliveCancelToken.Token);
            }
        }
        catch(Exception ex)
        {

        }
    }
    private void SetIconVisibility(VisibilityType type)
    {
        if (_iconTransition || _currentVisibility == type)
            return;
        switch (type)
        {
            case VisibilityType.VERY_FAR:
                OnVeryFar?.Invoke();
                break;
            case VisibilityType.NEAR:
                OnNear?.Invoke();
                break;
            case VisibilityType.FAR:
                OnFar?.Invoke();
                break;
        }
        _currentVisibility = type;
    }
}
