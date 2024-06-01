using System;
using Mirror;

namespace Gameplay.Multiplayer
{
    public class GameNetworkManager : NetworkManager
    {
        public event Action<NetworkConnectionToClient, CreateCharacterMessage> OnCreateCharacter;
        public string NickName { get; set; } = "Player";

        public override void OnStartServer()
        {
            base.OnStartServer();
            NetworkServer.RegisterHandler<CreateCharacterMessage>(CreateCharacter);
        }
        
        public override void OnClientConnect()
        {
            base.OnClientConnect();
            
            var characterMessage = new CreateCharacterMessage
            {
                name = NickName
            };

            NetworkClient.Send(characterMessage);
        }

        void CreateCharacter(NetworkConnectionToClient conn, CreateCharacterMessage message)
        {
            OnCreateCharacter?.Invoke(conn, message);
        }
    }
}