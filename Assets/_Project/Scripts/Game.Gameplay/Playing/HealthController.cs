using UnityEngine;
using System;

namespace Game.Gameplay.Playing
{
    public sealed class HealthController : MonoBehaviour, IHealthController
    {
        public event Action<float, float> OnHealthChanged;
        
        [SerializeField, Range(0f, 100f)] private float _maxHealth;

        private float _currentHealth;
        private float _shield;

        public void Initialize()
        {
            _currentHealth = _maxHealth;
        }
        
        public void Dispose()
        {}
        
        public void Add(float amount)
        {
            _currentHealth += amount;

            _currentHealth = Math.Clamp(_currentHealth, 0, _maxHealth);

            OnHealthChanged?.Invoke(_currentHealth, _maxHealth);
        }

        public void Remove(float amount)
        {
            _currentHealth -= amount;
            
            _currentHealth = Math.Clamp(_currentHealth, 0, _maxHealth);
            
            OnHealthChanged?.Invoke(_currentHealth, _maxHealth);
        }
    }
}