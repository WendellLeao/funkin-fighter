using Game.Services;
using UnityEngine;
using Game.Events;
using Game.Input;

namespace Game.Gameplay.Playing
{
    public sealed class PlayerManager : MonoBehaviour
    {
        [SerializeField] private Player _player;
        [SerializeField] private Player _playerBot;
        
        private IEventService _eventService;
        private IInputService _inputService;

        public Player Player => _player;

        public void Initialize()
        {
            _eventService = ServiceLocator.GetService<IEventService>();
            _inputService = ServiceLocator.GetService<IInputService>();
            
            _player.Begin(_eventService, _inputService);
            _playerBot.Begin(_eventService, _inputService);
        }

        public void Dispose()
        {
            _player.Stop();
        }

        public void Tick(float deltaTime)
        {
            _player.Tick(deltaTime);
        }
    }
}
