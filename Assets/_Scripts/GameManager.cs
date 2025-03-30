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

    public const string MAIN_MENU_SCENE_NAME = "Main menu";
    public const string END_SCREEN_SCENE_NAME = "EndScene";

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        StartCoroutine(GameSequence());
    }

    [ContextMenu("Quit Game")]
    public void QuitGame()
    {
        QuittingGame = true;
        ChangeScene(MAIN_MENU_SCENE_NAME);
    }

    public void MissionComplete()
    {
        MissionCompleted = true;
        ChangeScene(END_SCREEN_SCENE_NAME);
    }

    public void ChangeScene(string sceneName)
    {
        if (QuittingGame) return;

        Player.DisableInput();
        StartCoroutine(QuitGameCoroutine());

        IEnumerator QuitGameCoroutine()
        {
            BlackFadeController.Instance.FadeIn();
            yield return new WaitForSeconds(BlackFadeController.Instance.FadeDuration);

            LoadScene(sceneName);
        }
    }

    private void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
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
        }
        else
        {
            BlackFadeController.Instance.FadeOut();
        }
        

    }
}
