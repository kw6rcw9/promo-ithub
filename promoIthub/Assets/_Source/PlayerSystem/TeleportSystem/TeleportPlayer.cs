using System;
using System.Collections.Generic;
using BackendSystem.Core;
using BackendSystem.Repositories;
using BranchSystem;
using Cinemachine;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using ScoreSystem;
using UI;
using UnityEngine;
using Zenject;
using Timer = TimerSystem.Timer;
using R3;
using SoundSystem;

namespace PlayerSystem.TeleportSystem
{
    public class TeleportPlayer : MonoBehaviour
    {
        [Header("Extra Mechanic")] 
        [SerializeField] private int branchAmountToReduceTimer;
        [SerializeField] private CinemachineVirtualCamera camera;
        [SerializeField] private Vector2 endValue;
        [SerializeField] private float jumpPower;
        [SerializeField] private float duration;
        [SerializeField] private int scorePoints;
        [SerializeField] private List<Branch> prefabs;
        [Inject] private Timer timer;
        [SerializeField] private Rigidbody2D rb;
       
        [Inject] private BranchGenerator _branchGenerator;
        [Inject] private Score _score;
        [Inject] private LosePanelView _losePanelView;
        [Inject] private PlayerScoreController _playerScoreController;
        [SerializeField] private Animator animator;
        public ReactiveCommand<List<Branch>> NewGenerateBranchesEvent;
        public static Action LoseAction;
        public static Action GenerateAction;
        public static ReactiveCommand NewPlayer;
        private int _passBranchesAmount;
        private Queue<Branch> _queue;
        private Branch currBranch;
        private bool _firstGenerate = false;
        private Vector2 _leftD;
        private Vector2 _rightD; 
        private Vector2 _upD; 

        
        //Формируется очередь

        public void Init()
        {
            NewPlayer = new ReactiveCommand();
        }

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
                    endValue = currBranch.TeleportPosition.position;
                    _firstGenerate = false;
                }
            }
        }
        
        //Принимает вектор и сравнивает с позицией ветки в следующем элементе в очереди
       public  async UniTask Teleport(Vector2 value)
       {
           _passBranchesAmount++;
           if (_passBranchesAmount == branchAmountToReduceTimer)
           {
               timer.ReduceMaxTimer();
               print("REDUCED");
               _passBranchesAmount = 0;
           }
           if (value == new Vector2(0, 1))
           {
               if(!timer.enabled)
                   timer.enabled = true;
               Debug.Log(_queue.Peek().Type);
               if(_queue.Peek().Type == currBranch.Type)
               {
                   
                   currBranch = _queue.Dequeue();
             
                   //Debug.Log(currBranch.TeleportPosition.position - transform.position);
                   //transform.position = currBranch.TeleportPosition.position;
                   _score.IncreaseScore(scorePoints);
                   timer.Heal();
                   Jump();
                 
                   if (currBranch.IsCentered)
                   {
                       await _branchGenerator.GenerateBranchesAsync();
                   }

               }
               else
               {
                   
                   timer.Stop();
                   LoseAction?.Invoke();
                   DeathJump(new Vector2(0, 2));
                   await UniTask.Delay(2000, DelayType.DeltaTime);
                    //SendRecord(Score.ScoreCount);
                   _losePanelView.ShowPanel();
                   if(PlayerPrefs.HasKey("player"))
                        _playerScoreController.SendRecord(Score.ScoreCount);
                   else
                    NewPlayer.Execute(Unit.Default);
                       
                   
               }

           }
           else if (value == new Vector2(-1, 0))
           {
               if(!timer.enabled)
                timer.enabled = true;
               Debug.Log(_queue.Peek().Type);
               if(_queue.Peek().Type == BranchType.Left)
               {
                   currBranch = _queue.Dequeue();
                   
                   Debug.Log(currBranch.TeleportPosition.position - transform.position);
                   //transform.position = currBranch.TeleportPosition.position;
                   transform.rotation = new Quaternion(0,0,0, 0);
                   _score.IncreaseScore(scorePoints);
                   timer.Heal();
                   Jump();
                   if (currBranch.IsCentered)
                   {
                       await _branchGenerator.GenerateBranchesAsync();
                   }
               }
               else
               {
                   timer.Stop();
                   LoseAction?.Invoke();
                   if(currBranch.Type == BranchType.Left)
                    DeathJump(new Vector2(-2.56f, 2));
                   else
                   {
                       DeathJump(new Vector2(-4.56f, 2));
                   }
                   await UniTask.Delay(2000, DelayType.DeltaTime);
                   //SendRecord(Score.ScoreCount);
                   _losePanelView.ShowPanel();
                   if(PlayerPrefs.HasKey("player"))
                       _playerScoreController.SendRecord(Score.ScoreCount);
                   else
                       NewPlayer.Execute(Unit.Default);
               }
              
           }
           else
           {
               if(_queue.Peek().Type == BranchType.Right )
               {
                   if(!timer.enabled)
                       timer.enabled = true;
                   currBranch = _queue.Dequeue();
                   Debug.Log(currBranch.TeleportPosition.position - transform.position);
                   //transform.position = currBranch.TeleportPosition.position;
                   transform.rotation = new Quaternion(0,-180,0, 0);
                   _score.IncreaseScore(scorePoints);
                   timer.Heal();
                   Jump();
                   if (currBranch.IsCentered)
                   {
                         await _branchGenerator.GenerateBranchesAsync();
                   }
               }
               else
               {
                   print("death");
                   timer.Stop();
                   LoseAction?.Invoke();
                   if(currBranch.Type == BranchType.Right)
                       DeathJump(new Vector2(2.56f, 2));
                   else
                   {
                       DeathJump(new Vector2(4.56f, 2));
                   }
                   await UniTask.Delay(2000, DelayType.DeltaTime);
                   //SendRecord(Score.ScoreCount);
                  _losePanelView.ShowPanel();
                  if(PlayerPrefs.HasKey("player"))
                      _playerScoreController.SendRecord(Score.ScoreCount);
                  else
                      NewPlayer.Execute(Unit.Default);
               }
           }
       }

       /*async void SendRecord(long score)
       {
           
           PlayerPrefs.SetString("player", "Admin");
           if (PlayerPrefs.HasKey("player"))
           {
               try
               {
                    await _db.UpdateScoreIfHigherAsync(PlayerPrefs.GetString("player"), score);

               }
               catch (Exception e)
               {
                   Debug.Log("Ошибка при отправке рекорда: " + e.Message);
               }
           }
       }*/
       public void DeathJump(Vector2 vector)
       {
           
           endValue += vector;

           transform.DOJump(endValue, jumpPower, 1, duration)
               .OnComplete(DisableKinematic);

       }
       public void Jump()
       {
           
           //print($"fff: {(currBranch.TeleportPosition.position.x - endValue.x)}, {currBranch.TeleportPosition.position.y - endValue.y}");
           endValue = currBranch.TeleportPosition.position;
           print("jump");
           //rb.AddForce(new Vector2(speedLeft, speedUp));
           transform.DOJump(endValue, jumpPower, 1, duration);
       }

       
       private void DisableKinematic()
       {
           SoundManager.Instance.Play("Lose");
           rb.isKinematic = false;
           camera.Follow = null;
       }

    }
}
