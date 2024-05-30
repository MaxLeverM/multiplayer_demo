using Mirror;
using UnityEngine;

namespace Gameplay
{
    public class PlayerProfile : NetworkBehaviour
    {
        [SerializeField][SyncVar] private string _nickName;
        
        public string NickName
        {
            get => _nickName;
            set => _nickName = value;
        }
    }
}