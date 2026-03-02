using System;
using System.Collections.Generic;
using UnityEngine;
public sealed class PickableObject : MonoBehaviour
{
    [SerializeField] private string itemKey;

    public string ItemKey => itemKey;
}