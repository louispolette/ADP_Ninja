using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CinemachineCamera))]
public class CameraController : MonoBehaviour
{
    public static CameraController Instance { get; private set; }

    [field: Space]

    [field: SerializeField] public float SprintFOVMultiplier { get; private set; } = 1.25f;
    [field: SerializeField] public float SprintFOVIncreaseSpeed { get; private set; } = 2f;

    [field: SerializeField] public float SprintFOVDecreaseSpeed { get; private set; } = 2f;
    [field: SerializeField] public AnimationCurve SprintFOVChangeCurve { get; private set; }

    [SerializeField, Range(0f, 1f), Space] private float sprintFOVLerpValue;

    //[field: Header("References")]

    private float _baseFOV;
    private bool _hasSetInputCallbacks = false;

    private Coroutine _sprintFOVChangeCoroutine;

    public CinemachineCamera CinemachineCamera { get; private set; }
    public PlayerCameraTrackingTarget TrackingTarget { get; set; }

    public float SprintFOV => _baseFOV * SprintFOVMultiplier;

    private void OnEnable()
    {
        if (PlayerController.Instance != null)
        {
            AddPlayerInputCallbacks();
        }
    }

    private void OnDisable()
    {
        if (PlayerController.Instance != null)
        {
            RemovePlayerInputCallbacks();
        }
    }

    private void Awake()
    {
        Instance = this;

        CinemachineCamera = GetComponent<CinemachineCamera>();
        _baseFOV = CinemachineCamera.Lens.FieldOfView;
    }

    private void Start()
    {
        if (!_hasSetInputCallbacks)
        {
            AddPlayerInputCallbacks();
        }
    }

    public void EnableSprintEffect(InputAction.CallbackContext context)
    {
        DoSprintFOVChange(true);
    }

    public void DisableSprintEffect(InputAction.CallbackContext context)
    {
        DoSprintFOVChange(false);
    }

    private void AddPlayerInputCallbacks()
    {
        _hasSetInputCallbacks = true;

        PlayerController.Instance.SprintAction.started += EnableSprintEffect;
        PlayerController.Instance.SprintAction.canceled += DisableSprintEffect;
    }

    private void RemovePlayerInputCallbacks()
    {
        _hasSetInputCallbacks = false;

        PlayerController.Instance.SprintAction.started -= EnableSprintEffect;
        PlayerController.Instance.SprintAction.canceled -= DisableSprintEffect;
    }

    private void DoSprintFOVChange(bool enableSprintFOV)
    {
        if (_sprintFOVChangeCoroutine != null)
        {
            StopCoroutine(_sprintFOVChangeCoroutine);
        }
        
        _sprintFOVChangeCoroutine = StartCoroutine(DoSprintFOVChangeCoroutine());

        IEnumerator DoSprintFOVChangeCoroutine()
        {
            float targetLerpValue = (enableSprintFOV) ? 1f : 0f;

            while (sprintFOVLerpValue != targetLerpValue)
            {
                if (sprintFOVLerpValue < targetLerpValue)
                {
                    sprintFOVLerpValue += SprintFOVIncreaseSpeed * Time.deltaTime;
                }
                else
                {
                    sprintFOVLerpValue -= SprintFOVDecreaseSpeed * Time.deltaTime;
                }

                sprintFOVLerpValue = Mathf.Clamp01(sprintFOVLerpValue);

                float curveValue = SprintFOVChangeCurve.Evaluate(sprintFOVLerpValue);
                CinemachineCamera.Lens.FieldOfView = Mathf.Lerp(_baseFOV, SprintFOV, curveValue);
                yield return null;
            }
        }
    }

}
