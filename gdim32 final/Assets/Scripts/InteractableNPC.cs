using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableNPC : MonoBehaviour
{
    [SerializeField] private DialogueNode startingNode;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private float interactDistance = 2.0f;

    private bool playerInRange = false;
    void Update()
    {
        if (Vector3.Distance(transform.position, playerTransform.position) < interactDistance)
        {
            playerInRange = true;
        }
        else
        {
            playerInRange = false;
        }

        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            dialogueManager.StartDialogue(startingNode);
        }
    }
}
