using Mirror;

namespace Gameplay.Multiplayer
{
    public struct CreateCharacterMessage : NetworkMessage
    {
        public string name;
    }
}