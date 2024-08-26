using System;
using System.Collections;
using Cinemachine;
using CoreSystem;
using PlayerSystem.TeleportSystem;
using UI;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

namespace TimerSystem
{
    public class Timer: MonoBehaviour
    
    {
        [Inject] private LosePanelView _losePanelView;
    [Inject] private TimerView _timerView;
    [SerializeField] private CinemachineVirtualCamera camera;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float maxTime;
    [SerializeField]private float timeLeft;
    [SerializeField]private float initTimerVal;
    [SerializeField] private float heal;

    [Header("Extra Mechanic")]
    [SerializeField] private float reduceTimerAmount;
    [SerializeField] private float maxTimeLeft;
    private bool _isEnd = true;
    private bool _isRunning = true;

    private void Start()
    {
        timeLeft = initTimerVal;
        _timerView.UpdateTimerView(timeLeft, maxTime);
    }

    private void Update()
    {
        if (timeLeft > 0 && _isRunning)
        {
            timeLeft -= Time.deltaTime;
           _timerView.UpdateTimerView(timeLeft, maxTime);
        }
        else
        {
            if (_isEnd)
            {
                _isEnd = false;
                rb.isKinematic = false;
                camera.Follow = null;
                TeleportPlayer.LoseAction?.Invoke();
                Invoke("DisableKinematic", 1.5f);
                
            }
                
        }
    }

    public void ReduceMaxTimer()
    {
        if (maxTime <= maxTimeLeft)
            return;
        maxTime -= reduceTimerAmount;
    }
    
    public void Heal()
    {
        if (timeLeft < maxTime)
        {
            timeLeft += heal;
            _timerView.UpdateTimerView(timeLeft, maxTime);
                
        }
    }
    
        public void DisableKinematic()
        {
               print("какого хуя");
            _losePanelView.ShowPanel();
        }
        public void Stop()
        {
            _isRunning = false;
            _isEnd = false;
        }
    }
    
}