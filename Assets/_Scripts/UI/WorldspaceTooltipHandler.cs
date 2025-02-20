using System.Collections.Generic;
using UnityEngine;

public class WorldspaceTooltipHandler : MonoBehaviour
{
    [field: Space]

    [field: SerializeField] public WorldspaceIcon TooltipPrefab;

    private Dictionary<int,WorldspaceIcon> tooltips = new Dictionary<int,WorldspaceIcon>();

    private void Awake()
    {
        WorldspaceTooltips.Handler = this;
    }

    public void CreateTooltip(TooltipType tooltipType)
    {
        var newTooltip = Instantiate(TooltipPrefab, transform, false);
        //tooltips.Add(newTooltip);
    }
}

public static class WorldspaceTooltips
{
    public static WorldspaceTooltipHandler Handler { get; set; }

    /*public static int CreateTooltip(TooltipType tooltipType)
    {
        Handler.CreateTooltip(tooltipType);
    }*/
}
