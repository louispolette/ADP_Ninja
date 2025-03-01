using UnityEngine;

public class WorldspaceIcon : MonoBehaviour
{
    [field : SerializeField] public Transform FollowedTransform { get; set; }
    [field: SerializeField] public RectTransform CanvasRectTransform { get; set; }

    private Camera _mainCamera;
    private RectTransform _rectTransform;

    private Vector3 _previousFollowedTransformPosition;

    private void OnEnable()
    {
        CameraMoveDetection.onCameraMove += UpdateIconPosition;
    }

    private void OnDisable()
    {
        CameraMoveDetection.onCameraMove -= UpdateIconPosition;
    }

    private void Awake()
    {
        _mainCamera = Camera.main;
        _rectTransform = GetComponent<RectTransform>();
    }

    private void LateUpdate()
    {
        if (FollowedTransform.position != _previousFollowedTransformPosition)
        {
            UpdateIconPosition();
        }

        _previousFollowedTransformPosition = FollowedTransform.position;
    }

    private void UpdateIconPosition()
    {
        Vector3 screenPos = _mainCamera.WorldToViewportPoint(FollowedTransform.position);
        Vector3 dir = FollowedTransform.position - Camera.main.transform.position;
        float dot = Vector3.Dot(dir, Camera.main.transform.forward);

        if (dot < 0)
        {
            return;

            // ???????????????
            /*screenPos.x = -screenPos.x;
            screenPos.y = -screenPos.y;*/
        }

        _rectTransform.anchoredPosition = new Vector2(CanvasRectTransform.rect.width * screenPos.x,
                                                          CanvasRectTransform.rect.height * screenPos.y);
    }
}
