using System;
using TMPro;
using UnityEngine;

namespace Gameplay.UI.Player
{
    public class PlayerPanelView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _nicknameText;
        [SerializeField] private TMP_Text _healthText;
        [SerializeField] private string _healthFormat;
        
        [SerializeField] private PlayerProfile _playerProfile;
        [SerializeField] private Health _health;
        private void Start()
        {
            _playerProfile.NickName.AddListener(UpdateNickName);
            _health.HealthPoints.AddListener(UpdateHealth);
        }

        private void OnDestroy()
        {
            _playerProfile.NickName.RemoveListener(UpdateNickName);
            _health.HealthPoints.RemoveListener(UpdateHealth);
        }

        private void UpdateHealth(int health)
        {
            _healthText.text = string.Format(_healthFormat, health.ToString());
            
        }

        private void UpdateNickName(string nickname)
        {
            _nicknameText.text = nickname;
        }
    }
}