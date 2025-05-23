using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "InventoryItemData/Items/Create New Item")]
public class ItemData : ScriptableObject
{
    [Space]

    public string label;
    public int itemID;
    public Texture icon;
    public GameObject displayModel;

    [Space]

    [TextArea] public string description;
}
