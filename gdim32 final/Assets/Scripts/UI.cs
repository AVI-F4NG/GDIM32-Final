using UnityEngine;
using UnityEngine.UI;
using TMPro;

[DisallowMultipleComponent]
public sealed class UI : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private PlayerPickup pickupSource;

    [Header("Hint UI")]
    [SerializeField] private TMP_Text pickupHintText;
    [SerializeField, Min(0.1f)] private float hintRadius = 3.0f;
    [SerializeField] private LayerMask pickableMask = ~0;
    [SerializeField] private string hintMessage = "Left click to pick up";

    [Header("Quest Tick UI (Lantern only)")]
    [SerializeField] private Image questTickImage;

    [Header("Lantern UI")]
    [SerializeField] private string lanternItemKey = "Lantern";
    [SerializeField] private TMP_Text lanternSkillText;

    [Header("Glow Stone UI")]
    [SerializeField] private string glowStoneItemKey = "GlowStone";
    [SerializeField] private TMP_Text glowStoneCompletionText;

    private int glowCurrent;
    private int glowTarget = 3;

    private void Awake()
    {
        if (pickupSource == null) pickupSource = FindFirstObjectByType<PlayerPickup>();

        if (pickupHintText != null)
        {
            pickupHintText.text = hintMessage;
            pickupHintText.gameObject.SetActive(false);
        }

        if (questTickImage != null)
            questTickImage.gameObject.SetActive(false);

        if (lanternSkillText != null)
            lanternSkillText.gameObject.SetActive(false);

        // Always visible
        if (glowStoneCompletionText != null)
        {
            glowStoneCompletionText.gameObject.SetActive(true);

            if (pickupSource != null)
            {
                glowCurrent = pickupSource.GetQuestCount(glowStoneItemKey);
                glowTarget = Mathf.Max(1, pickupSource.GetQuestTarget(glowStoneItemKey));
            }

            glowStoneCompletionText.text = $"Completion: {glowCurrent}/{glowTarget}";
        }
    }

    private void OnEnable()
    {
        if (pickupSource == null) pickupSource = FindFirstObjectByType<PlayerPickup>();
        if (pickupSource != null) pickupSource.PickedUp += OnPickedUp;
    }

    private void OnDisable()
    {
        if (pickupSource != null) pickupSource.PickedUp -= OnPickedUp;
    }

    private void Update()
    {
        if (pickupHintText == null) return;

        bool nearPickable = IsNearAnyPickable();
        pickupHintText.gameObject.SetActive(nearPickable);

        if (nearPickable)
            pickupHintText.text = hintMessage;
    }

    private bool IsNearAnyPickable()
    {
        Vector3 center = transform.position;
        Collider[] hits = Physics.OverlapSphere(center, hintRadius, pickableMask, QueryTriggerInteraction.Ignore);

        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i] == null) continue;

            if (hits[i].TryGetComponent<PickableObject>(out _)) return true;
            if (hits[i].GetComponentInParent<PickableObject>() != null) return true;
        }

        return false;
    }

    private void OnPickedUp(PlayerPickup.PickupEvent e)
    {
        if (pickupHintText != null)
            pickupHintText.gameObject.SetActive(false);

        // Lantern: show lantern text + tick
        if (string.Equals(e.ItemKey, lanternItemKey, System.StringComparison.Ordinal))
        {
            if (lanternSkillText != null) lanternSkillText.gameObject.SetActive(true);
            if (questTickImage != null) questTickImage.gameObject.SetActive(true);
            return;
        }

        // GlowStone: always-visible completion text updates, no tick
        if (glowStoneCompletionText != null &&
            string.Equals(e.ItemKey, glowStoneItemKey, System.StringComparison.Ordinal) &&
            e.Mode == PlayerPickup.PickupMode.CountOnly)
        {
            glowCurrent = e.CurrentCount;
            glowTarget = Mathf.Max(1, e.TargetCount);
            glowStoneCompletionText.text = $"Completion: {glowCurrent}/{glowTarget}";
        }
    }
}