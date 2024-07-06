using BranchSystem;
using PlayerSystem.TeleportSystem;
using ScoreSystem;
using TimerSystem;
using UI;
using UnityEngine;
using Zenject;

namespace CoreSystem
{
    public class Installer : MonoInstaller

    {
        [SerializeField] private BranchGenerator generator;
        [SerializeField] private TeleportPlayer teleportPlayer;
        [SerializeField] private LosePanelView losePanelView;
        [SerializeField] private Timer timer;
        public override void InstallBindings()
        {
            Container.Bind<BranchGenerator>().FromInstance(generator).AsTransient().NonLazy();
            Container.Bind<TeleportPlayer>().FromInstance(teleportPlayer).AsTransient().NonLazy();
            Container.Bind<BranchPool>().AsTransient().NonLazy();
            Container.Bind<Score>().AsTransient().NonLazy();
            Container.Bind<Timer>().FromInstance(timer).AsTransient().NonLazy();
            Container.Bind<LosePanelView>().FromInstance(losePanelView).AsTransient().NonLazy();
        }
    }
}