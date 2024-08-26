using System;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using TimerSystem;
using Timer = TimerSystem.Timer;

namespace UI
{
    public class LosePanelView : MonoBehaviour
    {
        [SerializeField] private GameObject panel;
        [SerializeField] private GameObject timer;

        public void ShowPanel()
        {
            print("SHOW");
           
            panel.SetActive(true);
           panel.transform.DOMove(new Vector3(0, panel.transform.position.y - 7.2f ,0), 1.2f);
            timer.SetActive(false);
            
        }
    }
}
