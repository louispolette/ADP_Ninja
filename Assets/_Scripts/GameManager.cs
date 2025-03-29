using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [field: Space]

    [field: SerializeField] public PlayerController Player { get; private set; }

    [field: Space]

    [field: SerializeField] public bool DebugEnabled { get; private set; } = false;

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
            IntroUIController.Instance.Show();

            yield return new WaitForSeconds(6f);

            Player.Input.ActivateInput();
            IntroUIController.Instance.Hide();
        }
        

    }
}
