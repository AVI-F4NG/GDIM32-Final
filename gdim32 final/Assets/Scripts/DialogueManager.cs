using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class DialogueManager : MonoBehaviour
{
    public GameObject dialoguePanel;
    public TextMeshProUGUI currentDialogue;
    public Transform buttonContainer;
    public GameObject buttonPrefab;


    private void Start()
    {
        dialoguePanel.SetActive(false);
    }
    public void StartDialogue(DialogueNode startNode)
    {
        dialoguePanel.SetActive(true);
        UnityEngine.Cursor.lockState = CursorLockMode.None;
        UnityEngine.Cursor.visible = true;
        DisplayNode(startNode);
    }

    private void Update()
    {
        transform.LookAt(Camera.main.transform);
    }

    public void DisplayNode(DialogueNode node)
    {
        currentDialogue.text = node.dialogueText;
        foreach (Transform child in buttonContainer) Destroy(child.gameObject);

        if (node.options.Length == 0)
        {
            CreateButton("End Conversation", null);
        }
        else
        {
            foreach (var option in node.options)
            {
                CreateButton(option.buttonText, option.nextNode);
            }
        }
    }

    void CreateButton(string text, DialogueNode nextNode)
    {
        GameObject btnObj = Instantiate(buttonPrefab, buttonContainer);
        btnObj.GetComponentInChildren<TextMeshProUGUI>().text = text;
        btnObj.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => {
            if (nextNode != null) DisplayNode(nextNode);
            else EndDialogue();
        });
    }

    void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        UnityEngine.Cursor.lockState = CursorLockMode.Locked; // Re-lock for 3D play
        UnityEngine.Cursor.visible = false;
    }
}