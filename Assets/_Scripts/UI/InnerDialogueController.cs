using System.Collections;
using TMPro;
using UnityEngine;

public class InnerDialogueController : MonoBehaviour
{
    public static InnerDialogueController Instance { get; private set; }

    public TextMeshProUGUI Text { get; private set; }

    private Animator _animator;
    private Coroutine _innerDialogueCoroutine;

    private void Awake()
    {
        Instance = this;

        Text = GetComponentInChildren<TextMeshProUGUI>();
        _animator = GetComponent<Animator>();
    }

    public void ShowDialogue(string dialogueText, float duration)
    {
        Text.text = dialogueText;

        if (_innerDialogueCoroutine != null)
        {
            StopCoroutine(_innerDialogueCoroutine);
        }

        _innerDialogueCoroutine = StartCoroutine(InnerDialogueCoroutine(dialogueText, duration));
    }

    private IEnumerator InnerDialogueCoroutine(string dialogueText, float duration)
    {
        float elapsedTime = 0f;
        _animator.SetTrigger("show");

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        _animator.SetTrigger("hide");
    }

    [ContextMenu("Inner Dialogue Test")]
    private void InnerDialogueTest()
    {
        ShowDialogue("This is a test", 6f);
    }
}
