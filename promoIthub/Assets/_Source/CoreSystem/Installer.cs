using BranchSystem;
using PlayerSystem.TeleportSystem;
using UnityEngine;
using Zenject;

public class Installer : MonoInstaller
{
    [SerializeField] private BranchGenerator generator;
    [SerializeField] private TeleportPlayer teleportPlayer;
    public override void InstallBindings()
    {
        Container.Bind<BranchGenerator>().FromInstance(generator).AsTransient().NonLazy();
        Container.Bind<TeleportPlayer>().FromInstance(teleportPlayer).AsTransient().NonLazy();
        Container.Bind<BranchPool>().AsTransient().NonLazy();
    }
}