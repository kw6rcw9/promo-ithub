using System;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using TimerSystem;
using UnityEngine.UI;
using Timer = TimerSystem.Timer;

namespace UI
{
    public class LosePanelView : MonoBehaviour
    {
        [SerializeField] private RectTransform panel;
        [SerializeField] private Image fadePanel;
        [SerializeField] private GameObject timer;
        [SerializeField] private float duration = 0.5f;
        [SerializeField] private float fadeDuration = 0.5f;
        private Vector2 startPosition;
        private Vector2 targetPosition;
        [SerializeField] private Ease easeType = Ease.OutBounce; // Тип анимации

        private void Start()
        {
            // Запоминаем стартовую позицию (за экраном)
            startPosition = panel.anchoredPosition;
            // Конечная позиция - центр экрана
            targetPosition = new Vector2(startPosition.x, 0);

            // Начинаем с панели вне экрана
            panel.anchoredPosition = startPosition;
        }

        public void ShowPanel()
        {
            FadeIn();
            print("SHOW"); 
            panel.gameObject.SetActive(true); 
            panel.DOAnchorPos(targetPosition,duration).SetEase(easeType); 
            timer.SetActive(false);
            
        }
        
        private void FadeIn()
        {
            fadePanel.DOFade(0.5f, fadeDuration);
        }
        
        
    }
}
