using System.Collections.Generic;
using BackendSystem.Repositories;
using BranchSystem;
using Cysharp.Threading.Tasks;
using PlayerSystem.TeleportSystem;
using R3;
using ScoreSystem;
using TimerSystem;
using UI.LeaderboardSystem;
using UnityEngine;
using Zenject;

namespace CoreSystem
{
    public class Bootstrapper : MonoBehaviour
    {
        [SerializeField] private int startValueForTimer;
        [SerializeField] private int stepForHealTimer;
        private BranchGenerator _generator;
        private Timer _timer;
        private FireBaseRespository _db;
        public static ReactiveCommand<List<PlayerData>> UpdateLeaderboardCommand;
        private NewPlayerFormView _newPlayerFormView;
        private TeleportPlayer _teleportPlayer;
        [Inject]
        public void Construct(BranchGenerator generator, 
            Timer timer, FireBaseRespository db, NewPlayerFormView newPlayerFormView,
            TeleportPlayer teleportPlayer)
        {
            _teleportPlayer = teleportPlayer;
            _generator = generator;
            _timer = timer;
            _db = db;
            _newPlayerFormView = newPlayerFormView;
        }
 
        
        private void Awake()
        {
            UpdateLeaderboardCommand = new ReactiveCommand<List<PlayerData>>();
            Score.ScoreCount = 0;
            Time.timeScale = 1;
            _generator.Init();
            //StartCoroutine(_timer.StartTimer(startValueForTimer));
           // await _db.InitFirebaseAsync();

        }

        private async UniTaskVoid Start()
        {
            Debug.Log("Started");
            _teleportPlayer.Init();
            _newPlayerFormView.Init();
            var topPlayers = await _db.LoadTopPlayersAsync();
            UpdateLeaderboardCommand?.Execute(topPlayers);
    
        }
    }
}
