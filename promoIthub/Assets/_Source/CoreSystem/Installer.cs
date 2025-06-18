using BackendSystem.Core;
using BackendSystem.Repositories;
using BranchSystem;
using InputSystem;
using PlayerSystem.TeleportSystem;
using ScoreSystem;
using TimerSystem;
using UI;
using UI.LeaderboardSystem;
using UnityEngine;
using Zenject;

namespace CoreSystem
{
    public class Installer : MonoInstaller

    {
        [SerializeField] private InputListener inputListener;
        [SerializeField] private Animator animator;
        [SerializeField] private BranchGenerator generator;
        [SerializeField] private TeleportPlayer teleportPlayer;
        [SerializeField] private LosePanelView losePanelView;
        [SerializeField] private TimerView timerView;
        [SerializeField] private Timer timer;
        [SerializeField] private NewPlayerFormView newPlayerFormView;
        public override void InstallBindings()
        {
            
            Container.Bind<BranchGenerator>().FromInstance(generator).AsTransient().NonLazy();
            Container.Bind<NewPlayerFormView>().FromInstance(newPlayerFormView).AsTransient().NonLazy();
            Container.Bind<InputListener>().FromInstance(inputListener).AsTransient().NonLazy();
            Container.Bind<TeleportPlayer>().FromInstance(teleportPlayer).AsTransient().NonLazy();
            Container.Bind<BranchPool>().AsTransient().NonLazy();
            Container.Bind<FireBaseRespository>().AsSingle().NonLazy();
            Container.Bind<PlayerScoreController>().AsSingle().NonLazy();
            Container.Bind<Score>().AsTransient().NonLazy();
            Container.Bind<TimerView>().FromInstance(timerView).AsTransient().NonLazy();
            Container.Bind<Timer>().FromInstance(timer).AsTransient().NonLazy();
            Container.Bind<LosePanelView>().FromInstance(losePanelView).AsTransient().NonLazy();
            Container.Bind<Animator>().FromInstance(animator).AsTransient().NonLazy();
            
        }
    }
}