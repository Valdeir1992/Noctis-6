using UnityEngine;

[CreateAssetMenu(menuName ="Noctis/Leonora/Data")]
public class CharacterSO : ScriptableObject
{
    [SerializeField] private float _walkSpeed;
    [SerializeField] private float _runSpeed;
    [SerializeField] private float _jumpHeight;
    [SerializeField] private float _maxStamine;

    public float WalkSpeed { get => _walkSpeed;}
    public float RunSpeed { get => _runSpeed;}
    public float JumpHeight { get => _jumpHeight;}
    public float MaxStamine { get => _maxStamine;}
}
