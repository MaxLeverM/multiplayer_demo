using Gameplay.Multiplayer;
using UnityEngine;
using Zenject;

namespace Gameplay.Installers
{
    public class NetworkInstaller : MonoInstaller
    {
        [SerializeField] private GameNetworkManager _gameNetworkManager;
        public override void InstallBindings()
        {
            Container.Bind<GameNetworkManager>().FromInstance(_gameNetworkManager).AsSingle().NonLazy();
        }
    }
}