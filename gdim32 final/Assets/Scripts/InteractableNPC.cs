using UnityEngine;
using UnityEngine.SceneManagement;

public class InteractableNPC : MonoBehaviour
{
    [Header("Dialogue Nodes")]
    [SerializeField] private DialogueNode startingNode;
    [SerializeField] private DialogueNode afterLanternNode;
    [SerializeField] private DialogueNode afterAllGlowStonesNode;

    [Header("Interaction")]
    [SerializeField] private GameObject showInteract;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform npcTransform;
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private float interactDistance = 5.0f;
    [SerializeField] private PlayerGameplayBlockState blockState;

    [Header("Progression")]
    [SerializeField] private PlayerPickup playerPickup;
    [SerializeField] private string lanternItemKey = "Lantern";
    [SerializeField] private string glowStoneItemKey = "GlowStone";

    [Header("Win")]
    [SerializeField] private string winSceneName = "WinScene";

    private bool inConversation;
    private bool lanternObtainedCached;

    private void Awake()
    {
        if (blockState == null) blockState = PlayerGameplayBlockState.GetOrFind();
        if (playerPickup == null) playerPickup = FindFirstObjectByType<PlayerPickup>();
    }

    private void OnEnable()
    {
        if (dialogueManager != null)
            dialogueManager.DialogueEnded += OnDialogueEnded;

        if (playerPickup == null) playerPickup = FindFirstObjectByType<PlayerPickup>();
        if (playerPickup != null)
            playerPickup.PickedUp += OnPickedUp;
    }

    private void OnDisable()
    {
        if (dialogueManager != null)
            dialogueManager.DialogueEnded -= OnDialogueEnded;

        if (playerPickup != null)
            playerPickup.PickedUp -= OnPickedUp;
    }

    void Update()
    {
        if (blockState == null) blockState = PlayerGameplayBlockState.GetOrFind();

        bool inRange = Vector3.Distance(npcTransform.position, playerTransform.position) < interactDistance;

        if (inRange)
        {
            if (!showInteract.activeSelf && !inConversation)
                showInteract.SetActive(true);

            if (Input.GetKeyDown(KeyCode.E) && !inConversation)
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
        bool allGlowStones = HasAllGlowStones();

        if (allGlowStones && afterAllGlowStonesNode != null)
            return afterAllGlowStonesNode;

        if (lanternObtainedCached && !allGlowStones && afterLanternNode != null)
            return afterLanternNode;

        return startingNode;
    }

    private void OnDialogueEnded()
    {
        inConversation = false;
        if (blockState != null) blockState.SetTalking(false);

        if (lanternObtainedCached && HasAllGlowStones())
            SceneManager.LoadScene(winSceneName);
    }

    private bool HasAllGlowStones()
    {
        if (playerPickup == null) playerPickup = FindFirstObjectByType<PlayerPickup>();
        if (playerPickup == null) return false;

        int current = playerPickup.GetQuestCount(glowStoneItemKey);
        int target = Mathf.Max(1, playerPickup.GetQuestTarget(glowStoneItemKey));
        return current >= target;
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