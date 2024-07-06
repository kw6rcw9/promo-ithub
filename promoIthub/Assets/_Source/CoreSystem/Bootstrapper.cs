using System;
using System.Collections;
using System.Collections.Generic;
using BranchSystem;
using TimerSystem;
using UnityEngine;
using Zenject;

public class Bootstrapper : MonoBehaviour
{
    [SerializeField] private int startValueForTimer;
    [SerializeField] private int stepForHealTimer;
    private BranchGenerator _generator;
    private Timer _timer;
    [Inject]
    public void Construct(BranchGenerator generator, Timer timer)
    {
        _generator = generator;
        _timer = timer;
    }
    private void Awake()
    {
        Time.timeScale = 1;
        _generator.Init();
        //StartCoroutine(_timer.StartTimer(startValueForTimer));


    }
}
