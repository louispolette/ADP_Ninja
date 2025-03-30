using UnityEngine;

public class InventoryTutorialTrigger : MonoBehaviour
{
    public void ShowTutorial()
    {
        InnerDialogueController.Instance.ShowDialogue("Pres")
    }
}
