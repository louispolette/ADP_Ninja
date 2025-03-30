using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BlackFadeController : MonoBehaviour
{
    public static BlackFadeController Instance { get; private set; }

    [field: Space]

    [field: SerializeField] public float FadeDuration { get; private set; } = 1.5f;
    [field: SerializeField] public bool FadeOutOnStart { get; private set; } = false;

    public RawImage BlackScreen { get; private set; }

    private Coroutine _fadeCoroutine;

    private void Awake()
    {
        Instance = this;

        BlackScreen = GetComponentInChildren<RawImage>();

        SetBlackScreenOpacity(1f);
    }

    private void Start()
    {
        if (FadeOutOnStart)
        {
            FadeOut();
        }
    }

    [ContextMenu("FadeOut")]
    public void FadeOut()
    {
        SetBlackScreenOpacity(1f);
        _fadeCoroutine = StartCoroutine(BlackFadeCoroutine(0f));
    }

    [ContextMenu("FadeIn")]
    public void FadeIn()
    {
        SetBlackScreenOpacity(0f);
        _fadeCoroutine = StartCoroutine(BlackFadeCoroutine(1f));
    }

    private IEnumerator BlackFadeCoroutine(float targetOpacity)
    {
        float timeElapsed = 0f;
        float initialOpacity = BlackScreen.color.a;

        while (timeElapsed < FadeDuration)
        {
            SetBlackScreenOpacity(Mathf.Lerp(initialOpacity, targetOpacity, timeElapsed / FadeDuration));
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        SetBlackScreenOpacity(targetOpacity);
    }

    private void SetBlackScreenOpacity(float newOpacity)
    {
        Color newColor = BlackScreen.color;
        newColor.a = newOpacity;
        BlackScreen.color = newColor;
    }
}
