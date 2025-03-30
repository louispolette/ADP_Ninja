using Unity.Cinemachine;
using UnityEngine;

public class SpecialCameraZoneController : MonoBehaviour
{
    public CinemachineCamera VirtualCamera { get; private set; }

    private void Awake()
    {
        VirtualCamera = GetComponentInChildren<CinemachineCamera>();
    }

    private void Enable()
    {
        VirtualCamera.Priority.Value = 2;
    }

    private void Disable()
    {
        VirtualCamera.Priority.Value = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        Enable();
    }

    private void OnTriggerExit(Collider other)
    {
        Disable();
    }
}
