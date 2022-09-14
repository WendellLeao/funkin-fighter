using Game.Gameplay.Playing;
using Game.Gameplay.Notes;
using Game.Services;
using Game.Pooling;
using Game.Events;
using UnityEngine;

namespace Game.Gameplay
{
    public sealed class GameplaySystem : MonoBehaviour
    {
        [SerializeField] private PlayersManager _playersManager;
        [SerializeField] private NotesManager _notesManager;

        private void Awake()
        {
            IEventService eventService = ServiceLocator.GetService<IEventService>();
            IPoolingService poolingService = ServiceLocator.GetService<IPoolingService>();
            
            _notesManager.Initialize(eventService, poolingService);
            _playersManager.Initialize(eventService);
        }

        private void OnDestroy()
        {
            _playersManager.Dispose();
            _notesManager.Dispose();
        }

        private void Update()
        {
            _playersManager.Tick(Time.deltaTime);
            _notesManager.Tick(Time.deltaTime);
        }
    }
}
