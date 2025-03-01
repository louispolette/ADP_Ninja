using UnityEngine;

public struct TooltipInfo
{
    public TooltipType type;
    public string text;
    public Transform followedTransform;

    public TooltipInfo(TooltipType type, Transform followedTransform, string text)
    {
        this.type = type;
        this.followedTransform = followedTransform;
        this.text = text;
    }
}