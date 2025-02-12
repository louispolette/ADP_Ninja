using UnityEngine;

public class ItemController : MonoBehaviour, IInteractable
{
    [field : Space]

    [field : SerializeField] public ItemData ItemData { get; private set; }

    public void Interact()
    {
        
    }
}
