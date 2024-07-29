using System;
using System.Collections;
using CoreSystem;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

namespace TimerSystem
{
    public class Timer: MonoBehaviour
    
    {
    [Inject] private TimerView _timerView;
    [SerializeField] private float maxTime;
    [SerializeField]private float timeLeft;
    [SerializeField]private float initTimerVal;
    [SerializeField] private float heal;

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }
    }
}