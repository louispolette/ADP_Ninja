using UnityEngine;

[System.Serializable]
public class InventoryItem
{
    public InventoryItem(ItemData itemData, int amount)
    {
        this.itemData = itemData;
        this.amount = amount;
    }

    public ItemData itemData;
    public int amount;
}
