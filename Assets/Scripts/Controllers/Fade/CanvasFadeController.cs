using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class CanvasFadeController : MonoBehaviour, IFadeController
{
    [SerializeField] private Image _background;
    [SerializeField] private float _time;
    public async UniTask FadeIn()
    {
        for(float elapsedTime = 0;elapsedTime < 1.1f;elapsedTime += Time.deltaTime/_time)
        {
            _background.color = Color.Lerp(new Color(0, 0, 0, 1), new Color(0, 0, 0, 0), elapsedTime);
            await UniTask.NextFrame();
        }
    }

    public async UniTask FadeOut()
    {
        for (float elapsedTime = 0; elapsedTime < 1.1f; elapsedTime += Time.deltaTime / _time)
        {
            _background.color = Color.Lerp(new Color(0, 0, 0, 0), new Color(0, 0, 0, 1), elapsedTime);
            await UniTask.NextFrame();
        }
    }
}
