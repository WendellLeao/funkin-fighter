using Game.Gameplay.Playing;
using UnityEngine.UI;
using UnityEngine;

namespace Game.UI.Playing
{
    public sealed class PlayerStatusUI : MonoBehaviour
    {
        [SerializeField] private Image _healthBarImage;
        [SerializeField] private Image _specialBarImage;

        private Player _player;
        private HealthController _playerHealthController;

        public void Begin(Player player)
        {
            _player = player;

            _playerHealthController = _player.HealthController;

            _playerHealthController.OnHealthChanged += HandlePlayerHealthChanged;
        }

        public void Stop()
        {
            _playerHealthController.OnHealthChanged -= HandlePlayerHealthChanged;
        }

        public void Tick(float deltaTime)
        { }
        
        private void HandlePlayerHealthChanged(float currentHealth, float maxHealth)
        {
            float ratio = currentHealth / maxHealth;
            
            _healthBarImage.fillAmount = ratio;
        }
    }
}