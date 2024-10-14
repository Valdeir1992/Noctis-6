using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class CharacterCameraController : MonoBehaviour
{
    private int _currentCamera;
    private Cinemachine.CinemachineFreeLook _thirdPerson;
    private Cinemachine.CinemachineVirtualCamera _firstPerson;
    private Transform _target;
    [Inject] private ScreenWarningController _screenWarningController;

    public void Setup(Transform target)
    {
        _thirdPerson = GameObject.Find("ThirdPerson").GetComponent<Cinemachine.CinemachineFreeLook>();
        _firstPerson = GameObject.Find("FirstPerson").GetComponent<Cinemachine.CinemachineVirtualCamera>();
        _target = target;
        _thirdPerson.LookAt = _target;
        _thirdPerson.Follow = _target;
        _firstPerson.LookAt = _target;
        _firstPerson.Follow = _target;
    }
    private void Update()
    {
        var ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        if(Physics.Raycast(ray,out RaycastHit hit, 10))
        {
            if(hit.collider.TryGetComponent(out ItemFieldOfView view))
            {
                _screenWarningController.ShowMagnifying();
            }
            else
            {
                _screenWarningController.HiddenMagnfying();
            }
        }
        else
        {
            _screenWarningController.HiddenMagnfying();
        }

        if (Keyboard.current.cKey.wasPressedThisFrame)
        {
            var index = _currentCamera + 1;
            if (index > 1)
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