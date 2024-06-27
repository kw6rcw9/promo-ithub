using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Branch : MonoBehaviour
{
    [field: SerializeField] public Sprite BranchSprite { get; set; }
    [field: SerializeField] public BranchType Type { get; set; }
    [field: SerializeField] public Transform TeleportPosition { get; set; }
    [field: SerializeField]public bool IsCentered { get; set; } = false;
}
