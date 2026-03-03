using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI currentDialogue;
    public Transform buttonContainer;
    public GameObject buttonPrefab;

    public void StartDialogue(DialogueNode startNode)
    {
        DisplayNode(startNode);
    }

    private void Update()
    {
        transform.LookAt(Camera.main.transform);
    }

    void DisplayNode(DialogueNode node)
    {
        currentDialogue.text = node.dialogueText;

        // Clear previous buttons
        foreach (Transform child in buttonContainer) Destroy(child.gameObject);

        // Create a button for each branching path
        foreach (DialogueOption option in node.options)
        {
            GameObject btnObj = Instantiate(buttonPrefab, buttonContainer);
            btnObj.GetComponentInChildren<TextMeshProUGUI>().text = option.buttonText;

            // Set up the button click to load the next node
            btnObj.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => DisplayNode(option.nextNode));
        }
    }
}