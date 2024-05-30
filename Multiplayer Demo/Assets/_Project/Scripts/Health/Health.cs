using System;
using Mirror;
using UnityEngine;

namespace Gameplay
{
    public class Health : NetworkBehaviour
    {
        [SerializeField] [SyncVar] private int _maxHealth;
        [SerializeField] [SyncVar] private int _health;

        public event Action OnDeath;

        public void MakeDamage(int points)
        {
            if (!isServer)
                return;

            if (points <= 0)
                return;

            _health -= points;

            if (_health <= 0)
            {
                _health = 0;
                OnDeath?.Invoke();
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                if (isLocalPlayer)
                {
                    CmdTakeHealth(10);
                }
            }
        }
        
        [Command]
        void CmdTakeHealth(int value)
        {
            MakeDamage(value);
        }
    }
}