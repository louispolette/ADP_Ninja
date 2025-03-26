using UnityEngine;

public class PlayerCameraTrackingTarget : MonoBehaviour
{
    public Transform FollowedTransform { get; private set; }
    public Transform PlayerTransform { get; private set; }

    [field: SerializeField] public Transform TrackingTransform { get; private set; }
    [field: SerializeField] public Transform LookAtTransform { get; private set; }

    [field: Space]

    [field: SerializeField] public Vector3 FollowOffset { get; set; }

    [field: Space]

    [field: SerializeField, Range(0f, 1f)] public float JumpFollowRatio { get; private set; } = 0.4f;
    [field: SerializeField] public float JumpTrackingHeight { get; private set; } = 1f;
    [field: SerializeField] public float JumpLookAtHeight { get; private set; } = 1f;
    [field: SerializeField] public float HeightFollowSmoothTime { get; private set; } = 1f;

    public bool IsInJumpMode { get; private set; } = false;

    private float _jumpStartHeight;
    private Vector3 _targetTrackingPosition;
    private Vector3 _targetLookAtPosition;

    private Vector3 _trackingSmoothVelocity;
    private Vector3 _lookAtSmoothVelocity;

    private PointToFollow _pointToFollow;

    private enum PointToFollow
    {
        Player,
        JumpStartPosition
    }

    private void Awake()
    {
        PlayerTransform = transform.parent;
        FollowedTransform = PlayerTransform;

        transform.SetParent(null);
    }

    private void Start()
    {
        CameraController.Instance.TrackingTarget = this;
    }

    private void LateUpdate()
    {
        FollowPlayer();
        CopyRotation();
    }

    public void SetFollowedTransform(Transform newTransformToFollow)
    {
        FollowedTransform = newTransformToFollow;
    }

    private Vector3 GetNewTrackingPosition()
    {
        Vector3 point;

        switch (_pointToFollow)
        {
            case PointToFollow.Player:
                point = PlayerTransform.position;
                break;
            case PointToFollow.JumpStartPosition:
                point = PlayerTransform.position;
                point.y += JumpTrackingHeight;
                break;
            default:
                point = transform.position;
                break;
        }

        return point + FollowOffset;
    }

    private Vector3 GetNewLookAtPosition()
    {
        Vector3 point;

        switch (_pointToFollow)
        {
            case PointToFollow.Player:
                point = PlayerTransform.position;
                break;
            case PointToFollow.JumpStartPosition:
                point = PlayerTransform.position;
                point.y += JumpLookAtHeight;
                //point.y = Mathf.Lerp(JumpLookAtHeight, PlayerTransform.position.y, JumpFollowRatio);
                break;
            default:
                point = transform.position;
                break;
        }

        return point + FollowOffset;
    }

    private void FollowPlayer()
    {
        _targetTrackingPosition = GetNewTrackingPosition();
        _targetLookAtPosition = GetNewLookAtPosition();

        float trackingHeightSmoothed = Vector3.SmoothDamp(TrackingTransform.position, _targetTrackingPosition, ref _trackingSmoothVelocity, HeightFollowSmoothTime).y;
        float lookAtHeightSmoothed = Vector3.SmoothDamp(LookAtTransform.position, _targetLookAtPosition, ref _lookAtSmoothVelocity, HeightFollowSmoothTime).y;

        TrackingTransform.position = new Vector3(_targetTrackingPosition.x, trackingHeightSmoothed, _targetTrackingPosition.z);
        LookAtTransform.position = new Vector3(_targetLookAtPosition.x, lookAtHeightSmoothed, _targetLookAtPosition.z);

    }

    private void CopyRotation()
    {
        TrackingTransform.eulerAngles = new Vector3(transform.eulerAngles.x, FollowedTransform.eulerAngles.y, transform.eulerAngles.z);
        LookAtTransform.eulerAngles = new Vector3(transform.eulerAngles.x, FollowedTransform.eulerAngles.y, transform.eulerAngles.z);
    }

    public void StartJumpingMode()
    {
        IsInJumpMode = true;
        _pointToFollow = PointToFollow.JumpStartPosition;
        _jumpStartHeight = PlayerTransform.position.y;
    }

    public void StopJumpingMode()
    {
        IsInJumpMode = false;
        _pointToFollow = PointToFollow.Player;
    }
}