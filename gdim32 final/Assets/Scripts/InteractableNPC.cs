using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour
{
    public DialogueNode startingNode;
    public Transform playerTransform;
    public DialogueManager dialogueManager;
    public float interactDistance = 2.0f;

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
