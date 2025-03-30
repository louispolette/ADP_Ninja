using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [field: Space]

    [field: SerializeField] public PlayerController Player { get; private set; }

    [field: Space]

    [field: SerializeField] public bool DebugEnabled { get; private set; } = false;

    [field: Header("Timings")]

    [field: SerializeField] public float TimeBeforeFadeOut { get; private set; } = 1f;
    [field: SerializeField] public float IntroDuration { get; private set; } = 6f;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        StartCoroutine(GameSequence());
    }

    private IEnumerator GameSequence()
    {
        if (!DebugEnabled)
        {
            yield return null;

            Player.Input.DeactivateInput();
            Player.ResetMovementInput();

            yield return new WaitForSeconds(TimeBeforeFadeOut);

            BlackFadeController.Instance.FadeOut();

            yield return new WaitForSeconds(BlackFadeController.Instance.FadeDuration);

            IntroUIController.Instance.Show();

            yield return new WaitForSeconds(IntroDuration);

            Player.Input.ActivateInput();
            IntroUIController.Instance.Hide();
        }
        

    }
}
