using UnityEngine;

public class CollectibleController : MonoBehaviour, IInteractable
{
    [field: Space]

    [field: SerializeField] public bool Usable { get; set; } = true;

    [field: Space]

    [field : SerializeField] public ItemData ItemData { get; private set; }

    private Renderer _renderer;
    private Collider _collider;

    private TooltipSpawner _tooltipSpawner;

    public const string TOOLTIP_TEXT = "Collect";

    private void Awake()
    {
        _renderer = GetComponentInChildren<Renderer>();
        _collider = GetComponentInChildren<Collider>();
        _tooltipSpawner = GetComponent<TooltipSpawner>();
    }

    public void Interact()
    {
        Collect();
    }

    public void OnEnterInteractionRange()
    {
        _tooltipSpawner.SpawnTooltip();
    }

    public void OnExitInteractionRange()
    {
        _tooltipSpawner.RemoveTooltip();
    }

    private void Collect()
    {
        InventoryManager.Instance.AddItem(ItemData);

        _tooltipSpawner.RemoveTooltip();
        _tooltipSpawner.SpawnTooltips = false;
        Usable = false;
        _renderer.enabled = false;
        _collider.enabled = false;
        Destroy(gameObject, 3f);
    }
}
