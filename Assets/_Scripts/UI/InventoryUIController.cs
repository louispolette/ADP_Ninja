using System.Collections.Generic;
using UnityEngine;

public class InventoryUIController : MonoBehaviour
{
    [field : Space]

    [field : SerializeField] public CanvasGroup CanvasGroup { get; private set; }

    [field : Space]

    [field : SerializeField] public InventorySlotController[] InventorySlots {  get; private set; }

    private Canvas _canvas;

    private void Awake()
    {
        _canvas = GetComponent<Canvas>();
    }

    public void Show()
    {
        _canvas.enabled = true;
        UpdateSlots();
    }

    public void Hide()
    {
        _canvas.enabled = false;
    }

    [ContextMenu("Update Slots")]
    public void UpdateSlots()
    {
        List<InventoryItem> inventoryContent = InventoryManager.Instance.Content;

        for (int i = 0; i < InventorySlots.Length; i++)
        {
            if (i < inventoryContent.Count)
            {
                InventorySlots[i].SetData(inventoryContent[i]);
            }
            else
            {
                InventorySlots[i].SetData(null);
            }

            InventorySlots[i].UpdateGraphics();
        }
    }
}
