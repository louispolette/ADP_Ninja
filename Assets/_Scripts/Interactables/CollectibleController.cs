using System;
using UnityEngine;

public class CollectibleController : MonoBehaviour, IInteractable
{
    [field: Space]

    [field: SerializeField] public bool Usable { get; set; } = true;

    [field: Space]

    [field : SerializeField] public ItemData ItemData { get; private set; }
    [field: SerializeField] public AudioClip CollectSFX { get; private set; }

    private Renderer _renderer;
    private Collider _collider;

    private TooltipSpawner _tooltipSpawner;

    public Action OnCollect { get; set; }

    private void Awake()
    {
        _renderer = GetComponentInChildren<Renderer>();
        _collider = GetComponentInChildren<Collider>();
        _tooltipSpawner = GetComponent<TooltipSpawner>();
    }

    public void Interact()
    {
        Collect();
    }

    public void OnEnterInteractionRange()
    {
        _tooltipSpawner.SpawnTooltip();
    }

    public void OnExitInteractionRange()
    {
        _tooltipSpawner.RemoveTooltip();
    }

    private void PlayCollectSFX()
    {
        AudioSource.PlayClipAtPoint(CollectSFX, transform.position, 1f);
    }

    private void Collect()
    {
        InventoryManager.Instance.AddItem(ItemData);

        _tooltipSpawner.RemoveTooltip();
        _tooltipSpawner.SpawnTooltips = false;
        Usable = false;
        _renderer.enabled = false;
        _collider.enabled = false;
        OnCollect?.Invoke();
        InnerDialogueController.Instance.ShowDialogue($"{ItemData.name} added to inventory, press START to open", 5f);
        PlayCollectSFX();
        Destroy(gameObject, 1f);
    }
}
