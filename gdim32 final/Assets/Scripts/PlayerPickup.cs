using System;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public sealed class PlayerPickup : MonoBehaviour
{
    [Serializable]
    public sealed class HeldItemEntry
    {
        [Tooltip("Must match PickableObject.itemKey on the scene object.")]
        public string itemKey;

        [Tooltip("Inactive child GameObject under the player that represents the held item.")]
        public GameObject heldObject;
    }

    public readonly struct PickupEvent
    {
        public readonly string ItemKey;
        public readonly GameObject SceneObject;
        public readonly GameObject HeldObject;

        public PickupEvent(string itemKey, GameObject sceneObject, GameObject heldObject)
        {
            ItemKey = itemKey;
            SceneObject = sceneObject;
            HeldObject = heldObject;
        }
    }

    [Header("References")]
    [SerializeField] private Camera playerCamera;

    [Header("Raycast")]
    [SerializeField, Min(0.1f)] private float interactDistance = 4f;
    [SerializeField] private LayerMask interactMask = ~0;

    [Header("Held Items")]
    [SerializeField] private HeldItemEntry[] heldItems;

    public event Action<PickupEvent> PickedUp;

    private readonly Dictionary<string, GameObject> keyToHeldObject = new(StringComparer.Ordinal);
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
    }

    private void Update()
    {
        if (!Input.GetMouseButtonDown(0)) return;
        if (playerCamera == null) return;

        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out RaycastHit hit, interactDistance, interactMask, QueryTriggerInteraction.Ignore)){
            return;
        }
        
        if (!hit.transform.TryGetComponent(out PickableObject pickable))
            pickable = hit.transform.GetComponentInParent<PickableObject>();

        if (pickable == null) return;

        if (!keyToHeldObject.TryGetValue(pickable.ItemKey, out GameObject heldObj) || heldObj == null)
            return;

        if (activeHeld != null) activeHeld.SetActive(false);
        activeHeld = heldObj;
        activeHeld.SetActive(true);

        pickable.gameObject.SetActive(false);

        PickedUp?.Invoke(new PickupEvent(pickable.ItemKey, pickable.gameObject, activeHeld));
    }
}
