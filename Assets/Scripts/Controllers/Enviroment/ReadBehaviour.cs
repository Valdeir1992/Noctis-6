using Cysharp.Threading.Tasks;
using UnityEngine;

public sealed class ReadBehaviour : InteractBehaviour
{
    [SerializeField] private DocumentType _documentType;
    public override async UniTask Execute(params object[] items)
    {
        gameplayController.ReadDocument((ItemSO)items[0], _documentType);
    }
}
