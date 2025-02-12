using UnityEngine;
using UnityEngine.SceneManagement;

public class Play : MonoBehaviour
{
    public void OnChangeScene(int i)
    {
        SceneManager.LoadScene(i);
    }
}
