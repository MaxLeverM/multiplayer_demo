using Gameplay.Multiplayer;
using UnityEngine;
using Zenject;

namespace Gameplay.Installers
{
    public class NetworkInstaller : MonoInstaller
    {
        [SerializeField] private GameNetworkManager _gameNetworkManagerPrefab;
        public override void InstallBindings()
        {
            Container.Bind<GameNetworkManager>().FromComponentInNewPrefab(_gameNetworkManagerPrefab).AsSingle().NonLazy();
        }
    }
}