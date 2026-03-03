using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableNPC : MonoBehaviour
{
    [SerializeField] public DialogueNode startingNode;
    [SerializeField] public Transform playerTransform;
    [SerializeField] public DialogueManager dialogueManager;
    [SerializeField] public float interactDistance = 2.0f;

    private bool playerInRange = false;
    void Update()
    {
        if (Vector3.Distance(transform, playerTransform) < interactDistance)
        {
            playerInRange = true;
        }
        else
        {
            playerInRange = false;
        }

        if (PlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            dialogueManager.StartDialogue(startingNode);
        }
    }
}
