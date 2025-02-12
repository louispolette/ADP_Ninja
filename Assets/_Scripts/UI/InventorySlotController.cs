using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlotController : MonoBehaviour, ISelectHandler
{
    [field: Space]

    [field: SerializeField] public InventoryItem InventoryItemData { get; private set; }

    [field : Header("Self References")]

    [field : SerializeField] public RawImage Icon { get; private set; }
    [field: SerializeField] public TextMeshProUGUI AmountText { get; private set; }

    public static Action<InventoryItem> onItemSelected { get; set; }

    private Selectable _selectable;

    private void Awake()
    {
        _selectable = GetComponent<Selectable>();
    }

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

    public void OnSelect(BaseEventData eventData)
    {
        onItemSelected?.Invoke(InventoryItemData);
    }
}
