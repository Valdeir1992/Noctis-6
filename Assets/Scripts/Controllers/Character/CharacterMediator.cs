using ECM2;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMediator : MonoBehaviour
{
    private GameInputs _inputs;
    private Vector2 _currentDirectionInput;
    private Character _moviment;
    [SerializeField] private CharacterSO _data;

    private void Awake()
    {
        _moviment = GetComponent<Character>();
        SetupInputs();
        SetupCharacterData();
    }

    private void Update()
    {
        Vector3 movementDirection = Vector3.zero;

        movementDirection += Vector3.right * _currentDirectionInput.x;
        movementDirection += Vector3.forward * _currentDirectionInput.y; 

        if (_moviment.camera)
        {
            movementDirection
                = movementDirection.relativeTo(_moviment.cameraTransform);
        } 
         
        _moviment.SetMovementDirection(movementDirection);
    }
    private void SetupCharacterData()
    {
        _moviment.maxWalkSpeed = _data.WalkSpeed;
        _moviment.camera = Camera.main;
    }

    private void SetupInputs()
    {
        _inputs = new GameInputs();
        _inputs.Character.Move.performed += Move;
        _inputs.Character.Jump.started += ctx=>_moviment.Jump();
        _inputs.Character.Jump.canceled += ctx => _moviment.StopJumping();
        _inputs.Character.Enable();
    }
    private void Move(InputAction.CallbackContext ctx)
    {
        _currentDirectionInput = ctx.ReadValue<Vector2>();
    }
}