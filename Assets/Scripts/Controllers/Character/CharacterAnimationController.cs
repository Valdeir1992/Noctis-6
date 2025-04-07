using UnityEngine;
using Zenject;

public class CharacterAnimationController : MonoBehaviour
{
    [Inject(Id = "Leonora")] private CharacterMediator _characterMediator;

    private void OnEnable()
    {
        _characterMediator.OnAnimation?.AddListener(UpdateAnimation);
    }

    private void UpdateAnimation(LeonoraAnimationData data)
    {

    } 
}