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
    [SerializeField] private PlayerGameplayBlockState blockState;

    private bool inConversation = false;

    void Update()
    {
        if (blockState == null) blockState = PlayerGameplayBlockState.GetOrFind();

        if (Vector3.Distance(npcTransform.position, playerTransform.position) < interactDistance)
        {
            if (!showInteract.activeSelf && !inConversation) showInteract.SetActive(true);

            if (Input.GetKeyDown(KeyCode.E))
            {
                inConversation = true;
                if (blockState != null) blockState.SetTalking(true);

                showInteract.SetActive(false);
                dialogueManager.StartDialogue(startingNode);
            }
        }
        else
        {
            inConversation = false;
            if (blockState != null) blockState.SetTalking(false);

            if (showInteract.activeSelf) showInteract.SetActive(false);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(npcTransform.position, interactDistance);
    }
}