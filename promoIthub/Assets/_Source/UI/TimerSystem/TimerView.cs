using System;
using Cinemachine;
using DG.Tweening;
using UI;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace TimerSystem
{
    public class TimerView : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera camera;
        [SerializeField] private Image bar;
        [SerializeField] private Rigidbody2D rb;
        [Inject] private LosePanelView _losePanelView;
        private float _maxTimer = 10;
        private float _timeLeft;
        private float _initTimerVal = 5;
        private float _heal = 0.4f;
        
        public void UpdateTimerView(float timeLeft, float maxTimer)
        {
            bar.fillAmount = timeLeft / maxTimer;
        }

       

        public void DisableKinematic()
        {
           
            _losePanelView.ShowPanel();
        }
    }
}
