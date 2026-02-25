using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI dialogueTextDisplay;
    public Transform buttonContainer;
    public GameObject buttonPrefab;

    public void StartDialogue(DialogueNode startNode)
    {
        DisplayNode(startNode);
    }

    void DisplayNode(DialogueNode node)
    {
        dialogueTextDisplay.text = node.dialogueText;

        // Clear previous buttons
        foreach (Transform child in buttonContainer) Destroy(child.gameObject);

        // Create a button for each branching path
        foreach (DialogueOption option in node.options)
        {
            GameObject btnObj = Instantiate(buttonPrefab, buttonContainer);
            btnObj.GetComponentInChildren<TextMeshProUGUI>().text = option.buttonText;

            // Set up the button click to load the next node
            btnObj.GetComponent<Button>().onClick.AddListener(() => DisplayNode(option.nextNode));
        }
    }
}