using System;
using Gameplay.Installers;
using Gameplay.Multiplayer;
using Mirror;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace Gameplay
{
    public class PlayerSpawner : IDisposable
    {
        private readonly Player.Factory _playerFactory;
        private readonly GameNetworkManager _networkManager;
        private GameObject _playerPrefab;
        private readonly IDeathHandler<Health> _deathHandler;

        public PlayerSpawner(Player.Factory playerFactory, GameNetworkManager networkManager, [Inject(Id = GameInstaller.PlayerPrefabID)] GameObject playerPrefab, PlayerDeathHandler deathHandler)
        {
            _playerFactory = playerFactory;
            _networkManager = networkManager;
            _playerPrefab = playerPrefab;
            _deathHandler = deathHandler;

            _networkManager.OnCreateCharacter += SpawnOnServer;
            NetworkClient.RegisterPrefab(_playerPrefab, SpawnOnClient, UnSpawnOnClient);
        }

        [Server]
        public void SpawnOnServer(NetworkConnectionToClient conn, CreateCharacterMessage message)
        {
            var player = _playerFactory.Create();
            var playerProfile = player.GetComponent<PlayerProfile>();
            playerProfile.SetNickName(message.name);
            NetworkServer.AddPlayerForConnection(conn, player.gameObject);

            if (player.TryGetComponent(out Health health))
            {
                _deathHandler.AddObject(health);
            }
        }
        
        public GameObject SpawnOnClient(SpawnMessage msg)
        {
            return _playerFactory.Create().gameObject;
        }

        public void UnSpawnOnClient(GameObject spawned)
        {
            Object.Destroy(spawned);
        }

        public void Dispose()
        {
            _networkManager.OnCreateCharacter -= SpawnOnServer;
        }
    }
}