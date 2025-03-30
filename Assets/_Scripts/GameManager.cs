using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public bool MissionCompleted { get; private set; } = false;
    public bool QuittingGame { get; private set; } = false;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        StartCoroutine(GameSequence());
    }

    public void MissionComplete()
    {
        MissionCompleted = true;
    }

    [ContextMenu("Quit Game")]
    public void QuitGame()
    {
        if (QuittingGame) return;

        QuittingGame = true;
        Player.DisableInput();
        StartCoroutine(QuitGameCoroutine());

        IEnumerator QuitGameCoroutine()
        {
            BlackFadeController.Instance.FadeIn();
            yield return new WaitForSeconds(BlackFadeController.Instance.FadeDuration);

            LoadMainMenu();
        }
    }

    private void LoadMainMenu()
    {
        SceneManager.LoadScene("Main menu");
    }

    private IEnumerator GameSequence()
    {
        if (!DebugEnabled)
        {
            yield return null;

            Player.DisableInput();

            yield return new WaitForSeconds(TimeBeforeFadeOut);

            BlackFadeController.Instance.FadeOut();

            yield return new WaitForSeconds(BlackFadeController.Instance.FadeDuration);

            IntroUIController.Instance.Show();

            yield return new WaitForSeconds(IntroDuration);

            Player.EnableInput();
            IntroUIController.Instance.Hide();

            yield return new WaitUntil(() => MissionCompleted);
        }
        

    }
}
