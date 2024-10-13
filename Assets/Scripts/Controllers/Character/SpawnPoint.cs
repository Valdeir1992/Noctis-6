using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField] private string _name;

    public string Name { get => _name; set => _name = value; }
}