using UnityEngine;
using UnityEngine.SceneManagement;

public class Play : MonoBehaviour
{

    public void OnChangeScene()
    {
        BlackFadeController.Instance.FadeIn();
        Invoke("LoadScene", BlackFadeController.Instance.FadeDuration + 0.5f);
    }

    private void LoadScene()
    {
        SceneManager.LoadScene(1);
    }
}
