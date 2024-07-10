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
        private Branch currBranch;
        private bool _firstGenerate = false;
        
        //Формируется очередь
       

        public void SendNewBranches(List<Branch> list)
        {
            if (_queue == null)
            {
                _queue = new Queue<Branch>();
                _firstGenerate = true;
            }
            foreach (var item in list)
            {
                    
                _queue.Enqueue(item);
                if (_firstGenerate)
                {
                    currBranch = _queue.Dequeue();
                    Debug.Log(currBranch.transform.position);
                    _firstGenerate = false;
                }
            }
        }
        
        //Принимает вектор и сравнивает с позицией ветки в следующем элементе в очереди
       public  async UniTask Teleport(Vector2 value)
       {
           if (value == new Vector2(0, 1))
           {
               if(!timerView.enabled)
                   timerView.enabled = true;
               Debug.Log(_queue.Peek().Type);
               if(_queue.Peek().Type == currBranch.Type)
               {
                   currBranch = _queue.Dequeue();
                   transform.position = currBranch.TeleportPosition.position;
                   _score.IncreaseScore(scorePoints);
                   timerView.Heal();
                   if (currBranch.IsCentered)
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
           else if (value == new Vector2(-1, 0))
           {
               if(!timerView.enabled)
                timerView.enabled = true;
               Debug.Log(_queue.Peek().Type);
               if(_queue.Peek().Type == BranchType.Left && _queue.Peek().Type != currBranch.Type)
               {
                   currBranch = _queue.Dequeue();
                   transform.position = currBranch.TeleportPosition.position;
                   transform.rotation = new Quaternion(0,0,0, 0);
                   _score.IncreaseScore(scorePoints);
                   timerView.Heal();
                   if (currBranch.IsCentered)
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
               if(_queue.Peek().Type == BranchType.Right && _queue.Peek().Type != currBranch.Type)
               {
                   if(!timerView.enabled)
                       timerView.enabled = true;
                   currBranch = _queue.Dequeue();
                   transform.position = currBranch.TeleportPosition.position;
                   transform.rotation = new Quaternion(0,-180,0, 0);
                   _score.IncreaseScore(scorePoints);
                   timerView.Heal();
                   if (currBranch.IsCentered)
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
