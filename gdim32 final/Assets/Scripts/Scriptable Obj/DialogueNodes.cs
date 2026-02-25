using UnityEngine;

[CreateAssetMenu(fileName = "NewDialogueNode", menuName = "Dialogue/Node")]
public class DialogueNode : ScriptableObject
{
    [TextArea(3, 10)]
    public string dialogueText;
    public DialogueOption[] options;
}

[System.Serializable]
public struct DialogueOption
{
    public string buttonText;        // What the player clicks
    public DialogueNode nextNode;    // Where it leads
}