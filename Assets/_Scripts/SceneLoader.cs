using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    [field: Space]

    [field: SerializeField] public GameObject GraphicsParent { get; private set; }

    public static SceneLoader Instance;

    private const float MINIMUM_LOAD_TIME = 2f;

    private Slider _loadingBar;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        _loadingBar = GetComponentInChildren<Slider>(true);

        DontDestroyOnLoad(gameObject);
    }

    public void LoadScene(int sceneID)
    {
        StartCoroutine(LoadSceneAsync());

        IEnumerator LoadSceneAsync()
        {
            float loadStartTime = Time.time;
            float barVelocity = 0f;
            _loadingBar.value = 0f;
            EnableScreen();

            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneID);
            operation.allowSceneActivation = false;

            while (!operation.isDone)
            {
                float barProgress = Mathf.InverseLerp(0f, 0.9f, operation.progress);
                _loadingBar.value = Mathf.SmoothDamp(_loadingBar.value, barProgress, ref barVelocity, 0.25f);

                if (operation.progress >= 0.9f && Time.time - loadStartTime >= MINIMUM_LOAD_TIME)
                {
                    operation.allowSceneActivation = true;
                }

                yield return null;
            }

            _loadingBar.value = 1f;
            DisableScreen();
        }
    }

    private void EnableScreen()
    {
        GraphicsParent.SetActive(true);
    }

    private void DisableScreen()
    {
        GraphicsParent.SetActive(false);
    }
}
