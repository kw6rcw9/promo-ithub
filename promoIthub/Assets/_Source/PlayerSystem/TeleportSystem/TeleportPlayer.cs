using System;
using System.Collections.Generic;
using BranchSystem;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace PlayerSystem.TeleportSystem
{
    public class TeleportPlayer : MonoBehaviour
    {
        [SerializeField] private List<Branch> prefabs;
        [SerializeField] private GameObject panel;
        [Inject] private BranchGenerator _branchGenerator;
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
               Debug.Log(_queue.Peek().Type);
               if(_queue.Peek().Type == BranchType.Left)
               {
                   var branch = _queue.Dequeue();
                   transform.position = branch.TeleportPosition.position;
                   if (branch.IsCentered)
                   {
                       //GenerateAction?.Invoke();

                       await _branchGenerator.GenerateBranchesAsync();
                   }
               }
               else
               {
                   panel.SetActive(true);
                   Time.timeScale = 0;
                   LoseAction?.Invoke();
               }
              
           }
           else
           {
               if(_queue.Peek().Type == BranchType.Right)
               {
                   var branch = _queue.Dequeue();
                   transform.position = branch.TeleportPosition.position;
                   if (branch.IsCentered)
                   {
                         await _branchGenerator.GenerateBranchesAsync();
                   }
               }
               else
               {
                   panel.SetActive(true);
                   Time.timeScale = 0;
                   LoseAction?.Invoke();
               }
           }
       }
       
    }
}
