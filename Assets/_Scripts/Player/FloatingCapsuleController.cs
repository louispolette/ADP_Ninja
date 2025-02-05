using UnityEngine;

public class FloatingCapsuleController : MonoBehaviour
{
    [field: Space]

    [field: SerializeField] public float GroundDetectionRange { get; private set; } = 1f;
    [field: SerializeField] public float RideHeight { get; private set; } = 0.75f;
    [field: SerializeField] public LayerMask GroundDetectionLayerMask { get; private set; }

    [field: Header("Spring Values")]

    [field: SerializeField] public float SpringStrength { get; private set; } = 1f;
    [field: SerializeField] public float SpringDamper { get; private set; } = 1f;


    [Space]

    [SerializeField] private CapsuleCollider _collider;
    private Rigidbody _rb;

    private void Awake()
    {
        _collider = GetComponent<CapsuleCollider>();
        _rb = _collider.attachedRigidbody;
    }

    private void FixedUpdate()
    {
        Ray ray = new Ray(transform.TransformPoint(_collider.center), Vector3.down);

        if (Physics.Raycast(ray, out RaycastHit hit, GroundDetectionRange, GroundDetectionLayerMask, QueryTriggerInteraction.Ignore))
        {
            Vector3 vel = _rb.linearVelocity;
            Vector3 rayDir = Vector3.down;

            float rayDirVel = Vector3.Dot(rayDir, vel);

            float x = hit.distance - RideHeight;

            float springForce = (x * SpringStrength) - (rayDirVel * SpringDamper);

            _rb.AddForce(rayDir * springForce);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 center;

        if (_collider != null)
        {
            center = transform.TransformPoint(_collider.center);
        }
        else
        {
            center = transform.position;
        }

        Gizmos.color = Color.red;
        Gizmos.DrawLine(center, center + Vector3.down * GroundDetectionRange);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(center + Vector3.right * 0.1f, center + Vector3.down * RideHeight + Vector3.right * 0.1f);
    }
}
