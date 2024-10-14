using UnityEngine;

[CreateAssetMenu(menuName ="Noctis/Enviroment/Visibility")]
public class VisibilitySO : ScriptableObject
{
    [SerializeField] private float _farDistance;
    [SerializeField] private float _nearDistance;

    public float FarDistance { get => _farDistance;}
    public float NearDistance { get => _nearDistance;}
}
