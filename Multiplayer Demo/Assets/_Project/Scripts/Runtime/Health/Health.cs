using System;
using Mirror;
using UnityEngine;

namespace Gameplay
{
    public class Health : NetworkBehaviour, IDeathable<Health>

    {
        [SerializeField] [SyncVar] private int _maxHealth;

        [SerializeField] [SyncVar(hook = nameof(SyncUpdateHealth))]
        private int _health;

        public int MaxHealth => _maxHealth;
        public ReactiveProperty<int> HealthPoints;
        public event Action<Health> OnDeath;

        private void Awake()
        {
            UpdateHealth(_health);
        }

        [Server]
        public void MakeDamage(int points)
        {
            ChangeHealth(-Mathf.Abs(points));
        }

        [Server]
        public void RestoreHealth(int points)
        {
            ChangeHealth(Mathf.Abs(points));
        }

        [Server]
        public void ChangeHealth(int points)
        {
            if (!isServer)
                return;

            _health += points;
            if (_health >= _maxHealth)
            {
                _health = _maxHealth;
            }

            if (_health <= 0)
            {
                _health = 0;
                OnDeath?.Invoke(this);
            }

            UpdateHealth(_health);
        }

        [Command]
        public void CmdTakeHealth(int value)
        {
            MakeDamage(value);
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

        private void SyncUpdateHealth(int oldHealth, int newHealth)
        {
            UpdateHealth(newHealth);
        }

        private void UpdateHealth(int newHealth)
        {
            HealthPoints ??= new ReactiveProperty<int>(_health);
            HealthPoints.Value = newHealth;
        }
    }
}