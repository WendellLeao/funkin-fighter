﻿using UnityEngine;
using System;

namespace Game.Gameplay.Playing
{
    public sealed class HealthController : MonoBehaviour
    {
        [SerializeField] private float _maxHealth;

        private float _currentHealth;

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
        }

        public void Remove(float amount)
        {
            _currentHealth -= amount;
            
            _currentHealth = Math.Clamp(_currentHealth, 0, _maxHealth);
        }
    }
}