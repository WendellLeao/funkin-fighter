using Game.Gameplay.Animations;
using UnityEngine;
using Game.Events;
using Game.Input;

namespace Game.Gameplay.Playing
{
    public sealed class Player : Entity
    {
        [SerializeField] private PlayerInputsHandler _playerInputs;
        [SerializeField] private HealthController _healthController;
        [SerializeField] private DamageController _damageController;
        [SerializeField] private AnimationsController _animationsController;

        private IEventService _eventService;
        private int _index;
        
        public PlayerInputsHandler PlayerInputs => _playerInputs;
        public HealthController HealthController => _healthController;
        public int Index => _index;

        public void Begin(IEventService eventService, IInputService inputService, int index)
        {
            _eventService = eventService;
            _index = index;

            _playerInputs.Initialize(inputService, _eventService);
            _healthController.Initialize();
            _damageController.Initialize(_healthController, _eventService, _playerInputs);
            _animationsController.Initialize();
        }

        public void Stop()
        {
            _playerInputs.Dispose();
            _healthController.Dispose();
            _damageController.Dispose();
            _animationsController.Dispose();
        }

        public void Tick(float deltaTime)
        {
            _playerInputs.Tick(deltaTime);
        }
    }
}
