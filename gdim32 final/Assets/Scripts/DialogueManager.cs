using TMPro;
using UnityEngine;
using System;
using System.Collections;

public class DialogueManager : MonoBehaviour
{
    public GameObject dialoguePanel;
    public TextMeshProUGUI currentDialogue;
    public Transform buttonContainer;
    public GameObject buttonPrefab;
    public bool inConversation = false;
    [SerializeField] private float typingSpeed = 0.05f;

    public event Action DialogueEnded;

    private void Start()
    {
        dialoguePanel.SetActive(false);
    }

    public void StartDialogue(DialogueNode startNode)
    {
        inConversation = true;
        dialoguePanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        DisplayNode(startNode);
    }

    public void DisplayNode(DialogueNode node)
    {
        buttonContainer.gameObject.SetActive(false);
        StartCoroutine(TypeText(node.dialogueText, currentDialogue));
        foreach (Transform child in buttonContainer) Destroy(child.gameObject);

        if (node.options.Length == 0)
        {
            CreateButton("End Conversation", null);
        }
        else
        {
            foreach (var option in node.options)
                CreateButton(option.buttonText, option.nextNode);
        }
    }

    void CreateButton(string text, DialogueNode nextNode)
    {
        GameObject btnObj = Instantiate(buttonPrefab, buttonContainer);
        btnObj.GetComponentInChildren<TextMeshProUGUI>().text = text;
        btnObj.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
        {
            if (nextNode != null) DisplayNode(nextNode);
            else EndDialogue();
        });
    }

    void EndDialogue()
    {
        inConversation = false;
        dialoguePanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        DialogueEnded?.Invoke();
    }

    public IEnumerator TypeText(string textToType, TextMeshProUGUI textDisplay)
    {
        textDisplay.text = "";
        foreach (char letter in textToType.ToCharArray())
        {
            textDisplay.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        buttonContainer.gameObject.SetActive(true);
    }
}