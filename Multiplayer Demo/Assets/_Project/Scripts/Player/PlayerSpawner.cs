using Gameplay.Installers;
using Gameplay.Multiplayer;
using Mirror;
using UnityEngine;
using Zenject;

namespace Gameplay
{
    public class PlayerSpawner
    {
        private readonly Player.Factory _playerFactory;
        private readonly GameNetworkManager _networkManager;
        private GameObject _playerPrefab;

        public PlayerSpawner(Player.Factory playerFactory, GameNetworkManager networkManager, [Inject(Id = InputInstaller.PlayerPrefabID)] GameObject playerPrefab)
        {
            _playerFactory = playerFactory;
            _networkManager = networkManager;
            _playerPrefab = playerPrefab;

            _networkManager.OnCreateCharacter += SpawnOnServer;
            NetworkClient.RegisterPrefab(_playerPrefab, SpawnOnClient, UnSpawnOnClient);
        }

        public void SpawnOnServer(NetworkConnectionToClient conn, CreateCharacterMessage message)
        {
            var player = _playerFactory.Create();
            var playerProfile = player.GetComponent<PlayerProfile>();
            playerProfile.NickName = message.name;
            NetworkServer.AddPlayerForConnection(conn, player.gameObject);
        }
        
        public GameObject SpawnOnClient(SpawnMessage msg)
        {
            return _playerFactory.Create().gameObject;
        }

        public void UnSpawnOnClient(GameObject spawned)
        {
            Object.Destroy(spawned);
        }
    }
}