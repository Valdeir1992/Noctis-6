using ECM2;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class CharacterMediator : MonoBehaviour
{
    private GameInputs _inputs;
    private Vector2 _currentDirectionInput;
    private Character _moviment;
    private bool _canMove = true;
    private bool _canRun = true;
    [Inject] private GameplayController _gameplayController;
    [SerializeField] private CharacterSO _data;

    private void Awake()
    {
        _moviment = GetComponent<Character>();
        SetupInputs();
        SetupCharacterData();
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
        _inputs.Gameplay.Move.performed += Move;
        _inputs.Gameplay.Jump.started += ctx=>_moviment.Jump();
        _inputs.Gameplay.Jump.canceled += ctx => _moviment.StopJumping();
        _inputs.Gameplay.Enable();
    }
    private void Move(InputAction.CallbackContext ctx)
    {
        _currentDirectionInput = ctx.ReadValue<Vector2>();
    }
}