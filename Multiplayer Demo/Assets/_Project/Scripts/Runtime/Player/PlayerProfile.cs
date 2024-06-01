using Mirror;
using UnityEngine;

namespace Gameplay
{
    public class PlayerProfile : NetworkBehaviour
    {
        [SyncVar(hook = nameof(UpdateNickName))]
        [SerializeField] private string _nickName;

        public ReactiveProperty<string> NickName;

        private void Awake()
        {
            UpdateNickName(_nickName);
        }

        public void SetNickName(string newNickName)
        {
            _nickName = newNickName;
        }

        private void UpdateNickName(string oldName, string newName)
        {
            UpdateNickName(newName);
        }

        private void UpdateNickName(string newName)
        {
            NickName ??= new ReactiveProperty<string>(_nickName);
            NickName.Value = newName;
        }
    }
}