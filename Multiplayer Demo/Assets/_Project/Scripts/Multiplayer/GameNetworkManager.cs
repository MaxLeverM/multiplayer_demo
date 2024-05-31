using System;
using Mirror;
using UnityEngine;

namespace Gameplay.Multiplayer
{
    public class GameNetworkManager : NetworkManager
    {
        public event Action<NetworkConnectionToClient, CreateCharacterMessage> OnCreateCharacter;
        
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
                name = "Joe Gaba Gaba",
            };

            NetworkClient.Send(characterMessage);
        }

        void CreateCharacter(NetworkConnectionToClient conn, CreateCharacterMessage message)
        {
            OnCreateCharacter?.Invoke(conn, message);
        }
    }
    
    public struct CreateCharacterMessage : NetworkMessage
    {
        public string name;
    }
}