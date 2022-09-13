using System.Collections.Generic;
using Game.Gameplay.Playing;
using Game.UI.Playing;
using Game.Events;
using UnityEngine;

namespace Game.UI
{
    public sealed class PlayerUIManager : MonoBehaviour
    {
        [SerializeField] private PlayerStatusUI _playerStatusUIPrefab;
        [SerializeField] private Transform[] _spawnPoints;
        
        private List<PlayerStatusUI> _activeStatusUI;
        private IEventService _eventService;
        private int _playerIndex;

        public void Initialize(IEventService eventService)
        {
            _eventService = eventService;
        
            _activeStatusUI = new List<PlayerStatusUI>();

            _eventService.AddEventListener<PlayerCreatedEvent>(HandlePlayerCreated);
        }

        public void Dispose()
        {
            foreach (PlayerStatusUI playerStatusUI in _activeStatusUI)
            {
                playerStatusUI.Stop();
            }
            
            _eventService.RemoveEventListener<PlayerCreatedEvent>(HandlePlayerCreated);
        }

        public void Tick(float deltaTime)
        {
            foreach (PlayerStatusUI playerStatusUI in _activeStatusUI)
            {
                playerStatusUI.Tick(deltaTime);
            }
        }

        private void HandlePlayerCreated(ServiceEvent serviceEvent)
        {
            if (serviceEvent is PlayerCreatedEvent playerCreatedEvent)
            {
                Player player = playerCreatedEvent.Player;

                PlayerStatusUI playerStatusUI = Instantiate(_playerStatusUIPrefab, transform);

                playerStatusUI.transform.position = _spawnPoints[_playerIndex].position;
                
                playerStatusUI.Begin(player);
                
                _activeStatusUI.Add(playerStatusUI);

                _playerIndex += 1;
            }
        }
    }
}