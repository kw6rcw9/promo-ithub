using System;
using DG.Tweening;
using UI;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace TimerSystem
{
    public class TimerView : MonoBehaviour
    {
        [SerializeField] private Image bar;
        [Inject] private LosePanelView _losePanelView;
        private float _maxTimer = 10;
        private float _timeLeft;
        private float _initTimerVal = 5;
        private float _heal = 0.4f;

        private void Start()
        {
            _timeLeft = _initTimerVal;
            bar.fillAmount = _initTimerVal / 10;
        }
        // public void UpdateTimer(float val)
        // {
        //     // Debug.Log("Max:" + _maxTimer);
        //     //  var num = 1 - (val / _maxTimer);
        //     var num = bar.fillAmount - 0.1f;
        //      Debug.Log(num);
        //      bar.DOFillAmount(num, 0.3f);
        // }

        private void Update()
        {
            if (_timeLeft > 0)
            {
                _timeLeft -= Time.deltaTime;
                bar.fillAmount = _timeLeft / _maxTimer;
            }
            else
            {
                _losePanelView.ShowPanel();
            }
        }

        public void Heal()
        {
            if (_timeLeft < _maxTimer)
            {
                _timeLeft += _heal;
                bar.fillAmount = _timeLeft / _maxTimer;
                
            }
        }

        public void SetStartTimer(float val)
        {
            _maxTimer = val;
            bar.fillAmount = val / 10;
        }
    }
}
