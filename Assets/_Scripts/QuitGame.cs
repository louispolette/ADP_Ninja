using UnityEngine;

public class QuitGame : MonoBehaviour
{
    public AudioSource quitSound; 
    public float delayBeforeQuit = 1.0f; 
    public void Quit()
    {
        if (quitSound != null)
        {
            quitSound.Play();
            Invoke("ExitGame", delayBeforeQuit); 
        }
        else
        {
            ExitGame();
        }
    }

    void ExitGame()
    {
        Debug.Log("Le jeu se ferme...");
        Application.Quit();
    }
}