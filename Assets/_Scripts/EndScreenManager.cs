using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreenManager : MonoBehaviour
{
    public void BackToMenu()
    {
        BlackFadeController.Instance.FadeIn();
        Invoke("LoadMenuScene", BlackFadeController.Instance.FadeDuration + 0.5f);
    }

    private void LoadMenuScene()
    {
        SceneManager.LoadScene(0);
    }
}
