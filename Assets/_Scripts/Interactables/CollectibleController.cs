using UnityEngine;

public class CollectibleController : MonoBehaviour, IInteractable
{
    [field : Space]

    [field : SerializeField] public bool Usable { get; set; } = true;
    [field: SerializeField] public bool SpawnTooltips { get; set; } = true;

    [field : Space]

    [field : SerializeField] public Transform TooltipOrigin { get; private set; }

    [field: Space]

    [field : SerializeField] public ItemData ItemData { get; private set; }

    private Renderer _renderer;
    private Collider _collider;

    private TooltipController _createdTooltip;

    public const string TOOLTIP_TEXT = "Collect";

    private void Awake()
    {
        _renderer = GetComponentInChildren<Renderer>();
        _collider = GetComponentInChildren<Collider>();
    }

    public void Interact()
    {
        Collect();
    }

    public void OnEnterInteractionRange()
    {
        SpawnTooltip();
    }

    public void OnExitInteractionRange()
    {
        RemoveTooltip();
    }

    private void SpawnTooltip()
    {
        if (!SpawnTooltips) return;

        Transform tooltipLocation = (TooltipOrigin != null) ? TooltipOrigin : transform;
        TooltipInfo newTooltip = new TooltipInfo(TooltipType.Interact, tooltipLocation, TOOLTIP_TEXT);
        _createdTooltip = WorldspaceTooltips.CreateTooltip(newTooltip);
    }

    private void RemoveTooltip()
    {
        if (_createdTooltip == null) return;

        WorldspaceTooltips.RemoveTooltip(_createdTooltip);
        _createdTooltip = null;
    }

    private void Collect()
    {
        InventoryManager.Instance.AddItem(ItemData);

        RemoveTooltip();
        SpawnTooltips = false;
        Usable = false;
        _renderer.enabled = false;
        _collider.enabled = false;
        Destroy(gameObject, 3f);
    }
}
