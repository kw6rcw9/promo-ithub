using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PlayerSystem.TeleportSystem
{
    public class TeleportPlayer : MonoBehaviour
    {
        [SerializeField] private List<Branch> prefabs;
        [SerializeField] private GameObject panel;
        private Queue<Branch> _queue;
        
        //Формируется очередь
        void Start()
        {
            _queue = new Queue<Branch>();
            foreach (var item in prefabs)
            {
                _queue.Enqueue(item);
            }
        }
        
        //Принимает вектор и сравнивает с позицией ветки в следующем элементе в очереди
       public  void Teleport(Vector2 value)
       {
           if (value == new Vector2(-1, 0))
           {
               if(_queue.Peek().Type == BranchType.Left)
               {
                   var branch = _queue.Dequeue();
                   transform.position = branch.TeleportPosition.position;
               }
               else
               {
                   panel.SetActive(true);
                   Time.timeScale = 0;
               }
              
           }
           else
           {
               if(_queue.Peek().Type == BranchType.Right)
               {
                   var branch = _queue.Dequeue();
                   transform.position = branch.TeleportPosition.position;
                   
               }
               else
               {
                   panel.SetActive(true);
                   Time.timeScale = 0;
                   
               }
           }
       }
       
    }
}
