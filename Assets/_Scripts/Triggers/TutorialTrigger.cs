using System.Collections;
using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    [field: Space]

    [field: SerializeField] public float Duration { get; private set; } = 5f;

    [field: Space]
    [field: SerializeField, TextArea] public string[] TutorialText { get; private set; }

    private bool _isTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (_isTriggered) return;

        _isTriggered = true;

        StartCoroutine(TutorialTextCoroutine());

        IEnumerator TutorialTextCoroutine()
        {
            InnerDialogueController.Instance.ShowDialogue(TutorialText[0], Duration);

            yield return new WaitForSeconds(Duration + 1f);

            InnerDialogueController.Instance.ShowDialogue(TutorialText[1], Duration);
        }
    }
}
