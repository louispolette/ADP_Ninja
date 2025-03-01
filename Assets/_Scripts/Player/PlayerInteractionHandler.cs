using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerInteractionHandler : MonoBehaviour
{
    [Header("Debug")]

    [SerializeField] private Interactable[] _interactablesInRangeDebug;

    private PlayerController playerController;

    private HashSet<Interactable> _interactablesInRange = new HashSet<Interactable>();
    private HashSet<Interactable> _invalidInteractables = new HashSet<Interactable>();

    [System.Serializable]
    public class Interactable
    {
        public Transform transform;
        public IInteractable interactableInterface;

        public Interactable(Transform transform, IInteractable interactableInterface)
        {
            this.transform = transform;
            this.interactableInterface = interactableInterface;
        }
    }

    private void Awake()
    {
        playerController = GetComponentInParent<PlayerController>();
    }

    public void Interact()
    {
        Interactable closestInteractable = GetClosestInteractable();

        if (closestInteractable == null) return;

        closestInteractable.interactableInterface.Interact();
    }

    private Interactable GetClosestInteractable()
    {
        Interactable closestInteractable = null;
        float smallestDistance = 0f;

        foreach (var interactable in _interactablesInRange)
        {
            if (!interactable.interactableInterface.Usable)
            {
                _invalidInteractables.Add(interactable);
                continue;
            }

            if (closestInteractable == null)
            {
                closestInteractable = interactable;
                smallestDistance = GetDistanceFromPlayer(interactable);
                continue;
            }

            float distanceFromPlayer = GetDistanceFromPlayer(interactable);

            if (distanceFromPlayer < smallestDistance)
            {
                closestInteractable = interactable;
                smallestDistance = distanceFromPlayer;
            }
        }

        ClearInvalidInteractables();

        return closestInteractable;
    }

    private void ClearInvalidInteractables()
    {
        foreach (var invalidInteractable in _invalidInteractables)
        {
            _interactablesInRange.Remove(invalidInteractable);
        }

        _invalidInteractables.Clear();
        UpdateDebugInteractablesArray();
    }

    private float GetDistanceFromPlayer(Interactable interactable)
    {
        return Vector3.Distance(playerController.transform.position, interactable.transform.position);
    }

    private Interactable GetInteractable(IInteractable interactableInterface)
    {
        foreach (Interactable interactable in _interactablesInRange)
        {
            if (interactable.interactableInterface == interactableInterface)
            {
                return interactable;
            }
        }

        return null;
    }

    private void OnTriggerEnter(Collider other)
    {
        IInteractable interactable = other.GetComponentInParent<IInteractable>();

        if (interactable == null) return;
        
        interactable.OnEnterInteractionRange();
        _interactablesInRange.Add(new Interactable(other.transform, interactable));
        UpdateDebugInteractablesArray();
    }

    private void OnTriggerExit(Collider other)
    {
        IInteractable interactable = other.GetComponentInParent<IInteractable>();

        if (interactable == null) return;

        interactable.OnExitInteractionRange();
        _interactablesInRange.Remove(GetInteractable(interactable));
        UpdateDebugInteractablesArray();
    }

    private void UpdateDebugInteractablesArray()
    {
        _interactablesInRangeDebug = _interactablesInRange.ToArray();
    }
}
