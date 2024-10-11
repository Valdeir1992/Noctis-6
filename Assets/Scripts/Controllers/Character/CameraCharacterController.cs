using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraCharacterController : MonoBehaviour
{
    private int _currentCamera;
    [SerializeField] private CinemachineVirtualCamera _firstPerson;
    [SerializeField] private CinemachineFreeLook _thirdPerson;

    private void Update()
    {
        if (Keyboard.current.cKey.wasPressedThisFrame)
        {
            var index = _currentCamera +1;
            if(index > 1)
            {
                index = 0;
            } 
            ChangeCamera((CameraType)index);
        }
    }
    public void ChangeCamera(CameraType type)
    {
        _currentCamera = (int)type;
        ChangeAllPriority(10);
        switch (type)
        {
            case CameraType.First_Person:
                _firstPerson.Priority = 11;
                break;
            case CameraType.Third_Person:
                _thirdPerson.Priority = 11;
                break;
        }
    }
    private void ChangeAllPriority(int priority)
    {
        _firstPerson.Priority = priority;
        _thirdPerson.Priority = priority;
    }
}

public enum CameraType
{
    First_Person = 0,
    Third_Person = 1,
    Range_Attack = 2,
}
