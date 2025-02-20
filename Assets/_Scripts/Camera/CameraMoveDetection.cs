using System;
using UnityEngine;

public class CameraMoveDetection : MonoBehaviour
{
    public static Action onCameraMove;

    private Vector3 _previousCameraPosition;
    private Quaternion _previousCameraRotation;

    private void LateUpdate()
    {
        if (transform.position != _previousCameraPosition || transform.rotation != _previousCameraRotation)
        {
            onCameraMove?.Invoke();
        }

        _previousCameraPosition = transform.position;
        _previousCameraRotation = transform.rotation;
    }
}
