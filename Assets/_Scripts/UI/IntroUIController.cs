using UnityEngine;

public class IntroUIController : MonoBehaviour
{
    public static IntroUIController Instance { get; private set; }

    private Animator _animator;

    private void Awake()
    {
        Instance = this;

        _animator = GetComponent<Animator>();
    }

    public void Show()
    {
        _animator.SetTrigger("show");
    }

    public void Hide()
    {
        _animator.SetTrigger("hide");
    }
}
