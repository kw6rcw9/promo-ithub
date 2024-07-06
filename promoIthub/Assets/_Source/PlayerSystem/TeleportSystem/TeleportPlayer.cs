using System;
using System.Collections.Generic;
using BranchSystem;
using Cysharp.Threading.Tasks;
using ScoreSystem;
using TimerSystem;
using UI;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace PlayerSystem.TeleportSystem
{
    public class TeleportPlayer : MonoBehaviour
    {
        [SerializeField] private int scorePoints;
        [SerializeField] private List<Branch> prefabs;
        [SerializeField] private TimerView timerView;
        
        [Inject] private BranchGenerator _branchGenerator;
        [Inject] private Score _score;
        [Inject] private LosePanelView _losePanelView;
        public static Action LoseAction;
        public static Action GenerateAction;
        private Queue<Branch> _queue;
        
        //Формируется очередь
       

        public void SendNewBranches(List<Branch> list)
        {
            if (_queue == null)
                _queue = new Queue<Branch>();
            foreach (var item in list)
            {
                _queue.Enqueue(item);
            }
        }
        
        //Принимает вектор и сравнивает с позицией ветки в следующем элементе в очереди
       public  async UniTask Teleport(Vector2 value)
       {
           if (value == new Vector2(-1, 0))
           {
               if(!timerView.enabled)
                timerView.enabled = true;
               Debug.Log(_queue.Peek().Type);
               if(_queue.Peek().Type == BranchType.Left)
               {
                   var branch = _queue.Dequeue();
                   transform.position = branch.TeleportPosition.position;
                   _score.IncreaseScore(scorePoints);
                   timerView.Heal();
                   if (branch.IsCentered)
                   {
                       await _branchGenerator.GenerateBranchesAsync();
                   }
               }
               else
               {
                   _losePanelView.ShowPanel();
                   LoseAction?.Invoke();
               }
              
           }
           else
           {
               if(_queue.Peek().Type == BranchType.Right)
               {
                   if(!timerView.enabled)
                       timerView.enabled = true;
                   var branch = _queue.Dequeue();
                   transform.position = branch.TeleportPosition.position;
                   _score.IncreaseScore(scorePoints);
                   timerView.Heal();
                   if (branch.IsCentered)
                   {
                         await _branchGenerator.GenerateBranchesAsync();
                   }
               }
               else
               {
                  _losePanelView.ShowPanel();
                   LoseAction?.Invoke();
               }
           }
       }
       
    }
}
