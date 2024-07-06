using System;
using Unity.VisualScripting;
using UnityEngine;
using TimerSystem;
using Timer = TimerSystem.Timer;

namespace UI
{
    public class LosePanelView : MonoBehaviour
    {
        [SerializeField] private GameObject panel;

        private void OnEnable()
        {
            Timer.TimerLoseAction += ShowPanel;
        }

        private void OnDisable()
        {
            Timer.TimerLoseAction -= ShowPanel;
        }

        public void ShowPanel()
        {
            panel.SetActive(true);
            Time.timeScale = 0;
        }
    }
}
