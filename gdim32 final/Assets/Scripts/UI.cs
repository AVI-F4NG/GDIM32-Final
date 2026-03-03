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

    [Header("Quest Tick UI")]
    [SerializeField] private Image questTickImage;

    [Header("Lantern UI")]
    [SerializeField] private string lanternItemKey = "Lantern";
    [SerializeField] private TMP_Text lanternSkillText;

    private void Awake()
    {
        if (pickupSource == null) pickupSource = GetComponentInParent<PlayerPickup>();

        if (pickupHintText != null)
        {
            pickupHintText.text = hintMessage;
            pickupHintText.gameObject.SetActive(false);
        }

        if (questTickImage != null)
            questTickImage.gameObject.SetActive(false);

        if (lanternSkillText != null)
            lanternSkillText.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        if (pickupSource != null)
            pickupSource.PickedUp += OnPickedUp;
    }

    private void OnDisable()
    {
        if (pickupSource != null)
            pickupSource.PickedUp -= OnPickedUp;
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

        if (questTickImage != null)
            questTickImage.gameObject.SetActive(true);

        if (lanternSkillText != null && string.Equals(e.ItemKey, lanternItemKey, System.StringComparison.Ordinal))
            lanternSkillText.gameObject.SetActive(true);
    }
}