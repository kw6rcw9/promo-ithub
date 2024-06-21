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
        
        // Start is called before the first frame update
        void Start()
        {
            _queue = new Queue<Branch>();
            foreach (var item in prefabs)
            {
                _queue.Enqueue(item);
            }
        }

        // Update is called once per frame
        void Update()
        {
        
        }

       public  void Teleport(Vector2 value)
       {
           if (value == new Vector2(-1, 0))
           {
               if(_queue.Peek().Type == BranchType.Left)
               {
                   transform.position = new Vector3(-3.61f,3.58f, 0);
                   
               }
              
           }
           else
           {
               panel.SetActive(true);
               Time.timeScale = 0;
           }
       }

       public void Lose()
       {
           SceneManager.LoadScene(0);
       }
    }
}
