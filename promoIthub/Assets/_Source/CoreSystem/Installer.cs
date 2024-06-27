using BranchSystem;
using PlayerSystem.TeleportSystem;
using ScoreSystem;
using UnityEngine;
using Zenject;

namespace CoreSystem
{
    public class Installer : MonoInstaller

    {
        [SerializeField] private BranchGenerator generator;
        [SerializeField] private TeleportPlayer teleportPlayer;
        public override void InstallBindings()
        {
            Container.Bind<BranchGenerator>().FromInstance(generator).AsTransient().NonLazy();
            Container.Bind<TeleportPlayer>().FromInstance(teleportPlayer).AsTransient().NonLazy();
            Container.Bind<BranchPool>().AsTransient().NonLazy();
            Container.Bind<Score>().AsTransient().NonLazy();
        }
    }
}