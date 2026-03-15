using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcUIAnchor : MonoBehaviour
{
    [SerializeField] private Transform npcTransform;
    [SerializeField] private Vector3 offset = new Vector3(0, 2.2f, 0); // Height above NPC

    void LateUpdate()
    {
        if (npcTransform != null)
        {
            // Follow the NPC position + offset, but keep the UI's own rotation
            transform.position = npcTransform.position + offset;
        }
    }
}
