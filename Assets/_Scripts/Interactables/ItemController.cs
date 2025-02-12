using UnityEngine;

public class ItemController : MonoBehaviour, IInteractable
{
    [field : Space]

    [field : SerializeField] public ItemData ItemData { get; private set; }

    private Renderer _renderer;
    private Collider _collider;

    private void Awake()
    {
        _renderer = GetComponentInChildren<Renderer>();
        _collider = GetComponentInChildren<Collider>();
    }

    public void Interact()
    {
        Collect();
    }

    private void Collect()
    {
        InventoryManager.Instance.AddItem(ItemData);

        _renderer.enabled = false;
        _collider.enabled = false;
        Destroy(gameObject, 3f);
    }
}
