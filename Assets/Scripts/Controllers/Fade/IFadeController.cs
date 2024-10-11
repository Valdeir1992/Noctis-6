using Cysharp.Threading.Tasks;

public interface IFadeController
{
    public UniTask FadeIn();
    public UniTask FadeOut();
}
