using System.Collections.Generic;
using UnityEngine;

public class WorldspaceTooltipHandler : MonoBehaviour
{
    [field: Space]

    [field: SerializeField] public TooltipController TooltipPrefab { get; private set; }

    private HashSet<TooltipController> _tooltips = new HashSet<TooltipController>();
    private RectTransform _rectTransform;

    private void Awake()
    {
        WorldspaceTooltips.Handler = this;
        _rectTransform = GetComponent<RectTransform>();
    }

    public TooltipController CreateTooltip(TooltipInfo tooltipInfo)
    {
        var newTooltip = Instantiate(TooltipPrefab, transform, false);
        newTooltip.SetText(tooltipInfo.text);
        _tooltips.Add(newTooltip);
        
        var worldSpaceIcon = newTooltip.GetComponent<WorldspaceIcon>();
        worldSpaceIcon.FollowedTransform = tooltipInfo.followedTransform;
        worldSpaceIcon.CanvasRectTransform = _rectTransform;

        return newTooltip;
    }

    public void RemoveTooltip(TooltipController tooltipToRemove)
    {
        if (!_tooltips.Contains(tooltipToRemove))
        {
            Debug.LogError("Couldn't find tooltip to remove");
            return;
        }

        tooltipToRemove.FadeOut();
        tooltipToRemove.onTooltipFullyFadedOut += DestroyTooltip;
    }

    private void DestroyTooltip(TooltipController tooltipToRemove)
    {
        _tooltips.Remove(tooltipToRemove);
        Destroy(tooltipToRemove.gameObject);
    }
}