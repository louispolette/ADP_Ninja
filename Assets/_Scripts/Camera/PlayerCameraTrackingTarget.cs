using UnityEngine;

public class PlayerCameraTrackingTarget : MonoBehaviour
{
    public Transform FollowedTransform { get; private set; }
    public Transform PlayerTransform { get; private set; }


    public Vector3 FollowOffset { get; set; }
    private void Awake()
    {
        PlayerTransform = transform.parent;
        FollowedTransform = PlayerTransform;

        transform.SetParent(null);
    }

    private void LateUpdate()
    {
        FollowPlayer();
    }

    public void SetFollowedTransform(Transform newTransformToFollow)
    {
        FollowedTransform = newTransformToFollow;
    }

    private Vector3 GetNewPosition()
    {
        if (FollowedTransform == null)
        {
            return transform.position;
        }

        return FollowedTransform.position + FollowOffset;
    }

    private void FollowPlayer()
    {
        transform.position = GetNewPosition();
    }
}