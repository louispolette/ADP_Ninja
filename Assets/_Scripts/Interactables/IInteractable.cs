using System;
using UnityEngine;

public interface IInteractable
{
    public bool Usable { get; set; }
    public void Interact();
    public void OnEnterInteractionRange();
    public void OnExitInteractionRange();
}
