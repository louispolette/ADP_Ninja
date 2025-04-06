using UnityEngine;

public class InventoryItemRenderController : MonoBehaviour
{
    public static InventoryItemRenderController Instance { get; private set; }

    [field: Space]

    [field: SerializeField] public Transform DisplayItemParent { get; private set; }

    public GameObject CurrentDisplayedItem { get; private set; }

    private void Awake()
    {
         Instance = this;
    }

    public void Activate()
    {
        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }

    public void SetItemToDisplay(GameObject displayItemPrefab)
    {
        Destroy(CurrentDisplayedItem);
        CurrentDisplayedItem = Instantiate(displayItemPrefab, DisplayItemParent);
    }

    public void ClearItemDisplay()
    {
        Destroy(CurrentDisplayedItem);
        CurrentDisplayedItem = null;
    }
}
