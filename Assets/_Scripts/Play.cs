using UnityEngine;
using UnityEngine.SceneManagement;

public class Play : MonoBehaviour
{
    public void OnChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
