using UnityEngine;

public class InteractableNPC : MonoBehaviour
{
    [Header("Dialogue Nodes")]
    [SerializeField] private DialogueNode startingNode;
    [SerializeField] private DialogueNode afterLanternNode;       // one-line node
    [SerializeField] private DialogueNode afterAllGlowStonesNode; // final node

    [Header("Interaction")]
    [SerializeField] private GameObject showInteract;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform npcTransform;
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private float interactDistance = 5.0f;

    [Header("Progression")]
    [SerializeField] private PlayerPickup playerPickup;
    [SerializeField] private string lanternItemKey = "Lantern";
    [SerializeField] private string glowStoneItemKey = "GlowStone";

    [Header("Pause Flags")]
    [SerializeField] private PlayerGameplayBlockState blockState;

    private bool inConversation = false;
    private bool lanternObtainedCached = false;

    private void Awake()
    {
        if (blockState == null) blockState = PlayerGameplayBlockState.GetOrFind();
        if (playerPickup == null) playerPickup = FindFirstObjectByType<PlayerPickup>();
    }

    private void OnEnable()
    {
        if (playerPickup == null) playerPickup = FindFirstObjectByType<PlayerPickup>();
        if (playerPickup != null) playerPickup.PickedUp += OnPickedUp;
    }

    private void OnDisable()
    {
        if (playerPickup != null) playerPickup.PickedUp -= OnPickedUp;
    }

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
                dialogueManager.StartDialogue(SelectDialogueNode());
            }
        }
        else
        {
            inConversation = false;
            if (blockState != null) blockState.SetTalking(false);

            if (showInteract.activeSelf) showInteract.SetActive(false);
        }
    }

    private DialogueNode SelectDialogueNode()
    {
        if (playerPickup == null) playerPickup = FindFirstObjectByType<PlayerPickup>();

        bool allGlowStones = false;

        if (playerPickup != null)
        {
            int current = playerPickup.GetQuestCount(glowStoneItemKey);
            int target = Mathf.Max(1, playerPickup.GetQuestTarget(glowStoneItemKey));
            allGlowStones = current >= target;
        }

        if (allGlowStones && afterAllGlowStonesNode != null)
            return afterAllGlowStonesNode;

        if (lanternObtainedCached && !allGlowStones && afterLanternNode != null)
            return afterLanternNode;

        return startingNode;
    }

    private void OnPickedUp(PlayerPickup.PickupEvent e)
    {
        if (string.Equals(e.ItemKey, lanternItemKey, System.StringComparison.Ordinal))
            lanternObtainedCached = true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(npcTransform.position, interactDistance);
    }
}