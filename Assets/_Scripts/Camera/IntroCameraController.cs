using System;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class IntroCameraController : MonoBehaviour
{
    public static IntroCameraController Instance { get; private set; }

    [field: Space]

    [field: SerializeField] public Transform[] TrackingTargets { get; private set; }

    public Action OnIntroEnded { get; set; }

    private Animator _animator;
    private CinemachineCamera _virtualCamera;

    private void Awake()
    {
        Instance = this;

        _animator = GetComponent<Animator>();
        _virtualCamera = GetComponentInChildren<CinemachineCamera>();
    }

    public void StartIntroCameraTravelling()
    {
        _animator.SetTrigger("start");
        _virtualCamera.Priority.Value = 0;
    }

    public void SetTrackingTarget(int targetIndex)
    {
        var newTarget = _virtualCamera.Target;
        newTarget.TrackingTarget = TrackingTargets[targetIndex];
        _virtualCamera.Target = newTarget;
    }

    public void OnIntroEnd()
    {
        OnIntroEnded?.Invoke();
    }
}
