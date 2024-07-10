using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Branch : MonoBehaviour
{
    [SerializeField] private Sprite branchSprite;

    public Sprite BranchSprite
    {
        get => branchSprite;

        set
        {
            branchSprite = value;
            gameObject.GetComponent<SpriteRenderer>().sprite = BranchSprite;
        }
    }

    [field: SerializeField] public BranchType Type { get; set; }
    [field: SerializeField] public Transform TeleportPosition { get; set; }
    [field: SerializeField]public bool IsCentered { get; set; } = false;

    // private void OnEnable()
    // {
    //     gameObject.GetComponent<SpriteRenderer>().sprite = BranchSprite;
    //     
    // }
}
