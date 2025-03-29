using UnityEngine;

public class TooltipSpawner : MonoBehaviour
{
    [field: Space]

    [field: SerializeField] public bool SpawnTooltips { get; set; } = true;

    [field: SerializeField] public Transform TooltipOrigin { get; private set; }

    [field: SerializeField] public string TooltipText { get; private set; } = "Tooltip";

    private TooltipController _createdTooltip;

    public void SpawnTooltip()
    {
        if (!SpawnTooltips) return;

        Transform tooltipLocation = (TooltipOrigin != null) ? TooltipOrigin : transform;
        TooltipInfo newTooltip = new TooltipInfo(TooltipType.Interact, tooltipLocation, TooltipText);
        _createdTooltip = WorldspaceTooltips.CreateTooltip(newTooltip);
    }

    public void RemoveTooltip()
    {
        if (_createdTooltip == null) return;

        WorldspaceTooltips.RemoveTooltip(_createdTooltip);
        _createdTooltip = null;
    }
}
