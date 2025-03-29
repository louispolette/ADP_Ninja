using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    [field: Space]

    [field: SerializeField] public InventoryUIController UIController;

    [field: Space]

    [field : SerializeField] public List<InventoryItem> Content { get; private set; } = new List<InventoryItem>();

    [Space] public ItemData testItem;

    private void Awake()
    {
        Instance = this;
    }

    #region UI

    public void ShowInventoryUI()
    {
        UIController.Show();
    }

    public void HideInventoryUI()
    {
        UIController.Hide();
    }

    #endregion

    #region data

    private InventoryItem SearchForItemInInventory(int itemID)
    {
        foreach (var item in Content)
        {
            if (item.itemData.itemID == itemID)
            {
                return item;
            }
        }

        return null;
    }

    public void AddItem(ItemData itemData, int amountToAdd = 1)
    {
        InventoryItem inventoryItem = SearchForItemInInventory(itemData.itemID);

        if (inventoryItem != null)
        {
            inventoryItem.amount += amountToAdd;
        }
        else
        {
            Content.Add(new InventoryItem(itemData, amountToAdd));
        }
    }

    public void RemoveItem(ItemData itemData, int amountToRemove = 1)
    {
        InventoryItem inventoryItem = SearchForItemInInventory(itemData.itemID);

        if (inventoryItem == null) return;

        int newAmount = Content[itemData.itemID].amount - amountToRemove;

        if (newAmount > 0)
        {
            Content[itemData.itemID].amount = newAmount;
        }
        else
        {
            Content[itemData.itemID].amount = 0;
        }
    }
    
    public bool HasItem(int itemID)
    {
        InventoryItem inventoryItem = SearchForItemInInventory(itemID);

        return inventoryItem != null && inventoryItem.amount >= 1;
    }

    public bool HasItem(int itemID, int requiredAmount)
    {
        InventoryItem inventoryItem = SearchForItemInInventory(itemID);

        return inventoryItem != null && inventoryItem.amount >= requiredAmount;
    }

    [ContextMenu("AddItem Test")]
    public void AddTest()
    {
        AddItem(testItem);
    }

    [ContextMenu("RemoveItem Test")]
    public void RemoveTest()
    {
        RemoveItem(testItem, 2);
    }

    #endregion
}
