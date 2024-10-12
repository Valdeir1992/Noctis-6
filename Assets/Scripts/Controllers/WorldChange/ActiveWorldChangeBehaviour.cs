using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class ActiveWorldChangeBehaviour : MonoBehaviour
{
    [Inject] private WorldChangeController _worldChangeController;
    [SerializeField] private ActiveChange[] _changeWorld;

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
        var selected = _changeWorld.FirstOrDefault(x => x.World == world);
        if(selected != null)
        {
            for(int index = 0; index < transform.childCount; index++)
            {
                transform.GetChild(index).gameObject.SetActive(selected.Active);
            }
        }
    }
}
[System.Serializable]
public class ActiveChange : Change
{
    public bool Active;
}
