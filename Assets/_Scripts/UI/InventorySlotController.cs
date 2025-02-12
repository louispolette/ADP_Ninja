using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotController : MonoBehaviour
{
    [field: Space]

    [field: SerializeField] public InventoryItem InventoryItemData { get; private set; }

    [field : Header("Self References")]

    [field : SerializeField] public RawImage Icon { get; private set; }
    [field: SerializeField] public TextMeshProUGUI AmountText { get; private set; }

    public void SetData(InventoryItem data)
    {
        InventoryItemData = data;
    }

    public void UpdateGraphics()
    {
        if (InventoryItemData == null)
        {
            Icon.texture = null;
            Icon.enabled = false;
            AmountText.text = "";
            return;
        }

        Icon.enabled = true;

        if (InventoryItemData.itemData.icon != null)
        {
            Icon.texture = InventoryItemData.itemData.icon;
        }
        else
        {
            Icon.texture = null;
        }
        
        AmountText.text = (InventoryItemData.amount <= 0) ? "" : "x" + InventoryItemData.amount;
    }
}
