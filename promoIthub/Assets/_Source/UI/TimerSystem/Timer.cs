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
        [SerializeField] private TimerView timerView;
        public static Action TimerLoseAction;
        public float Value { get; set; }

        public IEnumerator StartTimer(int startVal)
        {
            Value = startVal;
            Debug.Log("Here");
            timerView.SetStartTimer(startVal);
            while (Value > 0)
            {
                DecreaseTimer(1);
                yield return new WaitForSecondsRealtime(1);
            }
           
        }
        public void IncreaseTimer(float val)
        {
            Value += val;
           
        }

        public void DecreaseTimer(float val)
        {
            Value -= val;
            Debug.Log(Value);
           //timerView.UpdateTimer(Value);
            if (Value < 0)
            {
                TimerLoseAction?.Invoke();
            }
                

        }
    }
}