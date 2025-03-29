using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class TooltipController : MonoBehaviour
{
    [field: Header("Animation")]
    [field: SerializeField] public float FadeDuration { get; private set; } = 0.2f;

    [field : Space]

    [field: SerializeField] public float HiddenSize { get; private set; } = 0.8f;
    [field: SerializeField] public AnimationCurve ScaleUpCurve { get; private set; }
    [field: SerializeField] public AnimationCurve ScaleDownCurve { get; private set; }

    private CanvasGroup _canvasGroup;
    private RectTransform _rectTransform;
    private TextMeshProUGUI _text;

    public Action<TooltipController> onTooltipFullyFadedOut;

    private Coroutine _fadeCoroutine;
    private TooltipVisibilityState _currentVisibilityState = TooltipVisibilityState.Hidden;

    private enum TooltipVisibilityState
    {
        Hidden = 0,
        Visible = 1,
        FadingIn = -1,
        FadingOut = -2,
    }

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _rectTransform = GetComponent<RectTransform>();
        _text = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Start()
    {
        FadeIn();
    }

    public void SetText(string newText)
    {
        _text.text = newText;
    }

    public void FadeOut()
    {
        DoFade(TooltipVisibilityState.Hidden);
    }

    public void FadeIn()
    {
        DoFade(TooltipVisibilityState.Visible);
    }

    private void DoFade(TooltipVisibilityState targetState)
    {
        if (targetState < 0)
        {
            Debug.LogError("Argument invalid, muste be either \"Hidden\" or \"Visible\"");
            return;
        }

        if (_fadeCoroutine != null)
        {
            StopCoroutine(_fadeCoroutine);
        }

        _fadeCoroutine = StartCoroutine(DoFadeCoroutine());

        IEnumerator DoFadeCoroutine()
        {
            float targetAlpha = (targetState == TooltipVisibilityState.Visible) ? 1f : 0f;
            float initialAlpha = _canvasGroup.alpha;

            Vector3 targetScale = (targetState == TooltipVisibilityState.Visible) ? Vector3.one : Vector3.one * HiddenSize;
            Vector3 initialScale = (targetState == TooltipVisibilityState.Visible) ? Vector3.one * HiddenSize : Vector3.one;
            AnimationCurve scaleCurve = (targetState == TooltipVisibilityState.Visible) ? ScaleUpCurve : ScaleDownCurve;

            float elapsedTime = 0f;

            _currentVisibilityState = (targetState == TooltipVisibilityState.Visible) ? TooltipVisibilityState.FadingIn : TooltipVisibilityState.FadingOut;

            while (elapsedTime < FadeDuration)
            {
                ChangeAlpha();
                ChangeScale();

                elapsedTime += Time.deltaTime;
                yield return null;

                void ChangeAlpha()
                {
                    _canvasGroup.alpha = Mathf.Lerp(initialAlpha, targetAlpha, elapsedTime / FadeDuration);
                }

                void ChangeScale()
                {
                    float lerp = scaleCurve.Evaluate(elapsedTime / FadeDuration);
                    _rectTransform.localScale = Vector3.LerpUnclamped(initialScale, targetScale, lerp);
                }
            }

            _canvasGroup.alpha = targetAlpha;
            _rectTransform.localScale = Vector3.one;
            _currentVisibilityState = targetState;

            if (targetState == TooltipVisibilityState.Hidden)
            {
                onTooltipFullyFadedOut?.Invoke(this);
            }
        }
    }
}
