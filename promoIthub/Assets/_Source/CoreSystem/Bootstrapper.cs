using System;
using System.Collections;
using System.Collections.Generic;
using BranchSystem;
using UnityEngine;
using Zenject;

public class Bootstrapper : MonoBehaviour
{
    private BranchGenerator _generator;
    [Inject]
    public void Construct(BranchGenerator generator)
    {
        _generator = generator;
    }
    private void Awake()
    {
        Time.timeScale = 1;
        _generator.Init();
    }
}
