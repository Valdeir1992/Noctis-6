using UnityEngine;
using Zenject;

public class CharacterInstaller : MonoInstaller
{
    [SerializeField] private CharacterSO _characterData;

    public override void InstallBindings()
    {
        Container.Bind<CharacterSO>().FromInstance(_characterData);
    }
}
