using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InteractableNPC : MonoBehaviour
{
    [SerializeField] private DialogueNode startingNode;
    [SerializeField] private GameObject showInteract;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform npcTransform;
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private float interactDistance = 5.0f;

    private bool inConversation = false;
    void Update()
    {
        if (Vector3.Distance(npcTransform.position, playerTransform.position) < interactDistance)
        {
            if (!showInteract.activeSelf && !inConversation) showInteract.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E))
            {
                inConversation = true;
                showInteract.SetActive(false);
                dialogueManager.StartDialogue(startingNode);
            }
        }
        else {
            inConversation = false;
            if (showInteract.activeSelf) showInteract.SetActive(false);
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(npcTransform.position, interactDistance);
    }
}
