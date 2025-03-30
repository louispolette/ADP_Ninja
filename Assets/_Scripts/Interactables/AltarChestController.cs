using UnityEngine;

public class AltarChestController : MonoBehaviour, IInteractable
{
    [field: Space]

    [field: SerializeField] public AudioClip InteractSound;

    public bool Usable { get; set; } = true;

    private TooltipSpawner _tooltipSpawner;

    private void Awake()
    {
        _tooltipSpawner = GetComponent<TooltipSpawner>();
    }

    public void Interact()
    {
        if (!Usable) return;

        AudioSource.PlayClipAtPoint(InteractSound, transform.position, 1f);
        GameManager.Instance.MissionComplete();
        _tooltipSpawner.RemoveTooltip();
    }

    public void OnEnterInteractionRange()
    {
        _tooltipSpawner.SpawnTooltip();
    }

    public void OnExitInteractionRange()
    {
        _tooltipSpawner.RemoveTooltip();
    }
}
