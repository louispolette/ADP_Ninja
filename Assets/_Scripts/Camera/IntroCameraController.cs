using System;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class IntroCameraController : MonoBehaviour
{
    public static IntroCameraController Instance { get; private set; }

    [field: Space]

    [field: SerializeField] public Transform SelfTrackingTarget { get; private set; }
    [field: SerializeField] public Transform[] TrackingTargets { get; private set; }

    public Action OnIntroEnded { get; set; }

    private Animator _animator;
    private CinemachineCamera _virtualCamera;
    private CinemachineSplineDolly _cinemachineSplineDolly;

    private void Awake()
    {
        Instance = this;

        _animator = GetComponent<Animator>();
        _virtualCamera = GetComponentInChildren<CinemachineCamera>();
        _cinemachineSplineDolly = GetComponentInChildren<CinemachineSplineDolly>();
    }

    public void StartIntroCameraTravelling()
    {
        _animator.SetTrigger("start");
    }

    public void SetTrackingTarget(int targetIndex)
    {
        var newTarget = _virtualCamera.Target;

        if (targetIndex < 0)
        {
            SelfTrackingTarget.transform.parent.forward = _virtualCamera.transform.forward;
            newTarget.TrackingTarget = SelfTrackingTarget;
        }
        else
        {
            newTarget.TrackingTarget = TrackingTargets[targetIndex];
        }

        _virtualCamera.Target = newTarget;
    }

    public void SetRotationMode(CinemachineSplineDolly.RotationMode newRotationMode)
    {
        _cinemachineSplineDolly.CameraRotation = newRotationMode;
    }

    public void OnIntroEnd()
    {
        _virtualCamera.Priority.Value = 0;
        OnIntroEnded?.Invoke();
    }

}
