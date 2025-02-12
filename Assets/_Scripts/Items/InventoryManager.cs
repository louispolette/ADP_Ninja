using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    [field: Space]
    
    [field : SerializeField] public int InventorySize { get; private set; } = 255;
    [field : SerializeField] public InventoryItem[] Content { get; private set; }

    [Space] public ItemData testItem;

    private bool _inventoryIsShown;

    private void Awake()
    {
        Instance = this;

        Content = new InventoryItem[InventorySize];
    }

    private bool CheckIfIDInRange(int ID)
    {
        if (ID >= Content.Length)
        {
            Debug.LogError($"Item ID {ID} is out of range");
            return false;
        }

        return true;
    }

    #region UI

    public void OnToggleMenuInput()
    {
        if (!_inventoryIsShown)
        {
            ShowInventoryUI();
        }
        else
        {
            HideInventoryUI();
        }
    }

    public void ShowInventoryUI()
    {
        _inventoryIsShown = true;
        Debug.Log("Inventory Shown");
    }

    public void HideInventoryUI()
    {
        _inventoryIsShown = false;
        Debug.Log("Inventory Hidden");
    }

    #endregion

    #region data

    public void AddItem(ItemData itemData, int amountToAdd = 1)
    {
        if (!CheckIfIDInRange(itemData.itemID)) return;

        Content[itemData.itemID].itemData = itemData;
        Content[itemData.itemID].amount += amountToAdd;
    }

    public void RemoveItem(ItemData itemData, int amountToRemove = 1)
    {
        if (!CheckIfIDInRange(itemData.itemID)) return;

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
    
    public bool HasItem(ItemData itemData)
    {
        if (!CheckIfIDInRange(itemData.itemID)) return false;

        return Content[itemData.itemID] != null && Content[itemData.itemID].amount >= 1;
    }

    public bool HasItem(ItemData itemData, int requiredAmount)
    {
        if (!CheckIfIDInRange(itemData.itemID)) return false;

        if (Content[itemData.itemID] == null) return false;

        return Content[itemData.itemID].amount >= requiredAmount;
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
