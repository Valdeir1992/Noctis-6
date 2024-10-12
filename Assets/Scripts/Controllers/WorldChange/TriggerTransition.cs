using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class TriggerTransition : MonoBehaviour
{
    [Inject] private WorldChangeController _worldChangeController;
    [SerializeField] private NoctisWorlds _transitionTo;


    private async void OnTriggerEnter(Collider other)
    {
        if (_worldChangeController.CurrentChange == _transitionTo)
            return;
        if(other.TryGetComponent(out CharacterMediator mediator))
        {
            _worldChangeController.SetCurrentWorld(_transitionTo);
            await _worldChangeController.Change();
        }
    }
}
