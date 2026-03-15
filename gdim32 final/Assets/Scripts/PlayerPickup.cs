using System;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public sealed class PlayerPickup : MonoBehaviour
{
    public enum PickupMode
    {
        HoldInHand,
        CountOnly
    }

    [Serializable]
    public sealed class HeldItemEntry
    {
        public string itemKey;
        public GameObject heldObject;
    }

    [Serializable]
    public sealed class QuestItemEntry
    {
        public string itemKey;
        public PickupMode mode = PickupMode.CountOnly;

        [Min(1)]
        public int targetCount = 3;
    }

    public readonly struct PickupEvent
    {
        public readonly string ItemKey;
        public readonly GameObject SceneObject;
        public readonly GameObject HeldObject;
        public readonly PickupMode Mode;

        public readonly int CurrentCount;
        public readonly int TargetCount;

        public PickupEvent(
            string itemKey,
            GameObject sceneObject,
            GameObject heldObject,
            PickupMode mode,
            int currentCount,
            int targetCount)
        {
            ItemKey = itemKey;
            SceneObject = sceneObject;
            HeldObject = heldObject;
            Mode = mode;
            CurrentCount = currentCount;
            TargetCount = targetCount;
        }
    }

    [Header("References")]
    [SerializeField] private Camera playerCamera;

    [Header("Raycast")]
    [SerializeField, Min(0.1f)] private float interactDistance = 4f;
    [SerializeField] private LayerMask interactMask = ~0;

    [Header("Hold Items (e.g., Lantern)")]
    [SerializeField] private HeldItemEntry[] heldItems;

    [Header("Quest Items (e.g., GlowStone)")]
    [SerializeField] private QuestItemEntry[] questItems;


    [Header("Pickup Audio")]
    [SerializeField] private AudioSource glowStoneSource;
    [SerializeField] private AudioSource lanternSource;

    public event Action<PickupEvent> PickedUp;

    private readonly Dictionary<string, GameObject> keyToHeldObject = new(StringComparer.Ordinal);
    private readonly Dictionary<string, QuestItemEntry> keyToQuestEntry = new(StringComparer.Ordinal);
    private readonly Dictionary<string, int> keyToCount = new(StringComparer.Ordinal);

    private GameObject activeHeld;

    private void Awake()
    {
        if (playerCamera == null) playerCamera = GetComponentInChildren<Camera>();

        keyToHeldObject.Clear();
        if (heldItems != null)
        {
            for (int i = 0; i < heldItems.Length; i++)
            {
                var entry = heldItems[i];
                if (entry == null) continue;
                if (string.IsNullOrWhiteSpace(entry.itemKey)) continue;
                if (entry.heldObject == null) continue;

                keyToHeldObject[entry.itemKey] = entry.heldObject;
                entry.heldObject.SetActive(false);
            }
        }

        keyToQuestEntry.Clear();
        keyToCount.Clear();
        if (questItems != null)
        {
            for (int i = 0; i < questItems.Length; i++)
            {
                var q = questItems[i];
                if (q == null) continue;
                if (string.IsNullOrWhiteSpace(q.itemKey)) continue;

                keyToQuestEntry[q.itemKey] = q;

                if (q.mode == PickupMode.CountOnly)
                    keyToCount[q.itemKey] = 0;
            }
        }
    }

    public int GetQuestCount(string itemKey)
    {
        return keyToCount.TryGetValue(itemKey, out int c) ? c : 0;
    }

    public int GetQuestTarget(string itemKey)
    {
        return keyToQuestEntry.TryGetValue(itemKey, out var q) ? Mathf.Max(0, q.targetCount) : 0;
    }

    public bool IsQuestComplete(string itemKey)
    {
        int target = GetQuestTarget(itemKey);
        return target > 0 && GetQuestCount(itemKey) >= target;
    }

    private void Update()
    {
        if (!Input.GetMouseButtonDown(0)) return;
        if (playerCamera == null) return;

        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out RaycastHit hit, interactDistance, interactMask, QueryTriggerInteraction.Ignore))
            return;

        if (!hit.transform.TryGetComponent(out PickableObject pickable))
            pickable = hit.transform.GetComponentInParent<PickableObject>();

        if (pickable == null) return;

        string key = pickable.ItemKey;
        if (string.IsNullOrWhiteSpace(key)) return;

        // -------------------------
        // QUEST ITEM (Glow Stone)
        // -------------------------
        if (keyToQuestEntry.TryGetValue(key, out QuestItemEntry quest) &&
            quest != null &&
            quest.mode == PickupMode.CountOnly)
        {
            int current = keyToCount.TryGetValue(key, out int c) ? c : 0;
            int target = Mathf.Max(1, quest.targetCount);

            if (current < target)
            {
                current++;
                keyToCount[key] = current;
            }

            pickable.gameObject.SetActive(false);

            // 🔊 Play Glow Stone sound
            if (glowStoneSource != null)
                glowStoneSource.Play();

            PickedUp?.Invoke(new PickupEvent(
                key,
                pickable.gameObject,
                heldObject: null,
                mode: PickupMode.CountOnly,
                currentCount: current,
                targetCount: target
            ));

            return;
        }

        // -------------------------
        // HOLD-IN-HAND ITEM (Lantern)
        // -------------------------
        if (!keyToHeldObject.TryGetValue(key, out GameObject heldObj) || heldObj == null)
            return;

        if (activeHeld != null) activeHeld.SetActive(false);
        activeHeld = heldObj;
        activeHeld.SetActive(true);

        pickable.gameObject.SetActive(false);

     
        if (lanternSource != null)
            lanternSource.Play();

        PickedUp?.Invoke(new PickupEvent(
            key,
            pickable.gameObject,
            heldObject: activeHeld,
            mode: PickupMode.HoldInHand,
            currentCount: 0,
            targetCount: 0
        ));
    }
}
