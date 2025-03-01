public static class WorldspaceTooltips
{
    public static WorldspaceTooltipHandler Handler { get; set; }

    public static TooltipController CreateTooltip(TooltipInfo tooltipInfo)
    {
        return Handler.CreateTooltip(tooltipInfo);
    }

    public static void RemoveTooltip(TooltipController tooltipToRemove)
    {
        Handler.RemoveTooltip(tooltipToRemove);
    }
}