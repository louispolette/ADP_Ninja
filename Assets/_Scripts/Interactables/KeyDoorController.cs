using UnityEngine;

public class KeyDoorController : MonoBehaviour, IInteractable
{
    [field: Space]

    [field: SerializeField] public Collider DoorCollision { get; private set; }

    public bool Usable { get; set; } = true;

    public bool IsOpen { get; private set; } = false;

    private TooltipSpawner _tooltipSpawner;
    private Animator _animator;

    private void Awake()
    {
        _tooltipSpawner = GetComponent<TooltipSpawner>();
        _animator = GetComponent<Animator>();
    }

    public void Interact()
    {
        if (IsOpen) return;

        bool hasKey = InventoryManager.Instance.HasItem(1);

        if (hasKey)
        {
            OpenDoor();
        }
    }



    public void OnEnterInteractionRange()
    {
        _tooltipSpawner.SpawnTooltip();
    }

    public void OnExitInteractionRange()
    {
        _tooltipSpawner.RemoveTooltip();
    }

    private void OpenDoor()
    {
        Usable = false;
        DoorCollision.enabled = false;
        _tooltipSpawner.RemoveTooltip();
        _tooltipSpawner.SpawnTooltips = false;
        _animator.SetTrigger("openDoor");
    }
}
