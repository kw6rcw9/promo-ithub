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

        public void ShowPanel()
        {
            panel.SetActive(true);
            Time.timeScale = 0;
        }
    }
}
