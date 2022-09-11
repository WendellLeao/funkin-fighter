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

        public Player Player => _player;

        public void Initialize()
        {
            IEventService eventService = ServiceLocator.GetService<IEventService>();
            IInputService inputService = ServiceLocator.GetService<IInputService>();
            
            _player.Begin(eventService, inputService);
            _playerBot.Begin(eventService, inputService);
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
