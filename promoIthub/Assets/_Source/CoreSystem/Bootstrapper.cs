using System.Collections.Generic;
using BackendSystem;
using BackendSystem.MetricSystem;
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
        [SerializeField] private GameObject mainPanel;
        [SerializeField] private GameObject player;
        [SerializeField] private GameObject timer;
        [SerializeField] private GameObject score;
        private Game _game;
        private BranchGenerator _generator;
        private Timer _timer;
        private FireBaseRespository _db;
        public static ReactiveCommand<List<PlayerData>> UpdateLeaderboardCommand;
        private NewPlayerFormView _newPlayerFormView;
        private TeleportPlayer _teleportPlayer;
        private FirebaseBridge _firebaseBridge;
        [Inject]
        public void Construct(BranchGenerator generator, 
            Timer timer, FireBaseRespository db, NewPlayerFormView newPlayerFormView,
            TeleportPlayer teleportPlayer, Game game, FirebaseBridge firebaseBridge)
        {
            _teleportPlayer = teleportPlayer;
            _generator = generator;
            _timer = timer;
            _db = db;
            _newPlayerFormView = newPlayerFormView;
            _game = game;
            _firebaseBridge = firebaseBridge;
        }
 
        
        private void Awake()
        {
            if (GameStateManager.Instance != null && GameStateManager.Instance.IsRestarted)
            {
                timer.SetActive(true);
                score.SetActive(true);
                mainPanel.SetActive(false);
                player.SetActive(true);
                _game.StartGame();

                // Сброс флага, чтобы не запускалось снова при следующем старте
                GameStateManager.Instance.ResetRestartFlag();
            }
            UpdateLeaderboardCommand = new ReactiveCommand<List<PlayerData>>();
            Score.ScoreCount = 0;
            Time.timeScale = 1;
            _generator.Init();
            FirebaseBridge.SendEvent("game_start", new { value = 1 });
            
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
