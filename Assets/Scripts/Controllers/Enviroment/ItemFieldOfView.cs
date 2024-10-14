using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class ItemFieldOfView : MonoBehaviour
{
    [SerializeField] private ItemSO _item;
   public void InteractView()
    {
        GetComponent<IEnviromentInteraction>().Execute(_item);
    }
}
