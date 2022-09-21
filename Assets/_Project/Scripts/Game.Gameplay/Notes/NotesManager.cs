using System.Collections.Generic;
using Game.Gameplay.Playing;
using Game.Pooling;
using Game.Events;
using UnityEngine;

namespace Game.Gameplay.Notes
{
    public sealed class NotesManager : MonoBehaviour
    {
        [SerializeField] private RectTransform[] _spawnPoints;
        [SerializeField] private NotesArea _notesAreaPrefab;

        private IPoolingService _poolingService;
        private IEventService _eventService;
        private List<NotesArea> _notesArea;
        private int _lastRandomIndex;

        public void Initialize(IEventService eventService, IPoolingService poolingService)
        {
            _eventService = eventService;
            _poolingService = poolingService;

            _notesArea = new List<NotesArea>();
            
            _eventService.AddEventListener<PlayerCreatedEvent>(HandlePlayerCreated);
        }

        public void Dispose()
        {
            foreach (NotesArea notesArea in _notesArea)
            {
                notesArea.Stop();
            }
            
            _eventService.RemoveEventListener<PlayerCreatedEvent>(HandlePlayerCreated);
        }

        public void Tick(float deltaTime)
        {
            foreach (NotesArea notesArea in _notesArea)
            {
                notesArea.Tick(deltaTime);
            }
        }

        private void HandlePlayerCreated(ServiceEvent serviceEvent)
        {
            if (serviceEvent is PlayerCreatedEvent playerCreatedEvent)
            {
                Player player = playerCreatedEvent.Player;

                RectTransform spawnPoint = _spawnPoints[player.Index];
                
                NotesArea notesArea = Instantiate(_notesAreaPrefab, spawnPoint);

                notesArea.Begin(player.NotesExecutor, _eventService);

                _notesArea.Add(notesArea);
            }
        }
    }
}