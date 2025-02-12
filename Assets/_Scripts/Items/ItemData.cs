using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Data/Items/Create New Item")]
public class ItemData : ScriptableObject
{
    [Space]

    public string label;
    public int itemID;

    [Space]

    [TextArea] public string description;
}
