using Game.Gameplay.Animations;
using Game.Events;
using UnityEngine;

namespace Game.Gameplay.Playing
{
    public sealed class Player : Entity
    {
        [SerializeField] private NotesExecutorBase _notesExecutor;
        [SerializeField] private HealthController _healthController;
        [SerializeField] private DamageController _damageController;
        [SerializeField] private AnimationsController _animationsController;
        
        private IEventService _eventService;
        private int _index;
        
        public int Index => _index;
        public INotesExecutor NotesExecutor => _notesExecutor;
        public HealthController HealthController => _healthController;
        
        public void Begin(IEventService eventService, int index)
        {
            _eventService = eventService;
            _index = index;

            _notesExecutor.Initialize(_eventService, _index);
            _healthController.Initialize();
            _damageController.Initialize(_healthController, _eventService, _notesExecutor);
            _animationsController.Initialize();
        }
        
        public void Stop()
        {
            _notesExecutor.Dispose();
            _healthController.Dispose();
            _damageController.Dispose();
            _animationsController.Dispose();
        }

        public void Tick(float deltaTime)
        {
            _notesExecutor.Tick(deltaTime);
        }
    }
}
