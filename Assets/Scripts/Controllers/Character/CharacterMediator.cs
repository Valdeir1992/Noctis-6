using ECM2;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using Zenject;

public class CharacterMediator : MonoBehaviour
{
    public UnityEvent<LeonoraAnimationData> OnAnimation;

    private bool _canMove = true;
    private bool _canRun = true;
    private GameInputs _inputs;
    private Vector2 _currentDirectionInput;
    private LeonoraAnimationData _leonoraAnimationData;
    [Inject(Id = "Leonora")] private Character _moviment;  
    [Inject] private GameplayController _gameplayController;
    [Inject] private CharacterSO _data; 

    private void Awake()
    {   
        SetupInputs();
        SetupCharacterData();
        _leonoraAnimationData = new LeonoraAnimationData();
    }
    private void OnEnable()
    {
        _gameplayController.OnPause += Pause;
        _gameplayController.OnResume += Resume;
    }
    private void OnDisable()
    {
        _gameplayController.OnResume -= Resume;
        _gameplayController.OnPause -= Pause;
    }
    private void Resume()
    {
        _canMove = true;
    }
    private void Pause()
    {
        _canMove = false;
    }
    private void Update()
    {
        Vector3 movementDirection = Vector3.zero;

        if (_canMove)
        { 
            movementDirection += Vector3.right * _currentDirectionInput.x;
            movementDirection += Vector3.forward * _currentDirectionInput.y;

            if (_moviment.camera)
            {
                movementDirection
                    = movementDirection.relativeTo(_moviment.cameraTransform);
            }
         }
        _leonoraAnimationData.Speed = movementDirection.magnitude; 
        _moviment.SetMovementDirection(movementDirection);
        OnAnimation?.Invoke(_leonoraAnimationData);
    }

    public void ToggleMove(bool v)
    {
        _canMove = v;  
    }

    private void SetupCharacterData()
    {
        _moviment.maxWalkSpeed = _data.WalkSpeed;
        _moviment.camera = Camera.main;
    }

    private void SetupInputs()
    {
        _inputs = new GameInputs();
        _inputs.Gameplay.Move.performed += Move;
        _inputs.Gameplay.Jump.started += Jump;
        _inputs.Gameplay.Jump.canceled += JumpEnd;
        _inputs.Gameplay.Enable();
    }

    private void Jump(InputAction.CallbackContext context)
    {
        _moviment.Jump();
        _leonoraAnimationData.Jump = true;
    }
    private void JumpEnd(InputAction.CallbackContext context)
    {
        _moviment.StopJumping();
        _leonoraAnimationData.Jump = false;
    }
    private void Move(InputAction.CallbackContext ctx)
    {
        _currentDirectionInput = ctx.ReadValue<Vector2>();
    }

    public class Factory : PlaceholderFactory<CharacterMediator>
    {

    }
}

public class LeonoraAnimationData
{
    public bool Run;
    public bool Jump;
    public float Speed;
}
