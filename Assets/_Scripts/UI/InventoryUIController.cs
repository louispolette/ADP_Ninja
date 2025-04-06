using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUIController : MonoBehaviour
{
    [field : Space]

    [field : SerializeField] public CanvasGroup CanvasGroup { get; private set; }

    [field : Space]

    [field : SerializeField] public InventorySlotController[] InventorySlots {  get; private set; }
    [field : Space]
    [field: SerializeField] public TextMeshProUGUI ItemNameText { get; private set; }
    [field : SerializeField] public TextMeshProUGUI ItemDescriptionText { get; private set; }
    [field : SerializeField] public RawImage ItemDisplayImage { get; private set; }

    private Canvas _canvas;

    private void Awake()
    {
        _canvas = GetComponent<Canvas>();
    }

    private void Start()
    {
        InventoryItemRenderController.Instance.Deactivate();
        ItemDisplayImage.enabled = false;

        UpdateItemDisplay(InventorySlots[0].InventoryItemData);
    }

    private void OnEnable()
    {
        InventorySlotController.onItemSelected += UpdateItemDisplay;
    }

    private void OnDisable()
    {
        InventorySlotController.onItemSelected -= UpdateItemDisplay;
    }

    public void Show()
    {
        InventoryItemRenderController.Instance.Activate();
        _canvas.enabled = true;
        UpdateSlots();
    }

    public void Hide()
    {
        InventoryItemRenderController.Instance.Deactivate();
        _canvas.enabled = false;
    }

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

    private void ClearItemDisplay()
    {
        ItemNameText.text = "";
        ItemDescriptionText.text = "";
        ItemDisplayImage.enabled = false;
    }

    private void UpdateItemDisplay(InventoryItem inventoryItemData)
    {
        if (inventoryItemData == null || inventoryItemData.itemData == null)
        {
            ClearItemDisplay();
            return;
        }

        ItemData itemData = inventoryItemData.itemData;

        ItemNameText.text = itemData.name;
        ItemDescriptionText.text = itemData.description;

        if (itemData.displayModel != null)
        {
            ItemDisplayImage.enabled = true;
            InventoryItemRenderController.Instance.SetItemToDisplay(itemData.displayModel);
        }
        else
        {
            ItemDisplayImage.enabled = false;
            InventoryItemRenderController.Instance.ClearItemDisplay();
        }
        
    }
}
