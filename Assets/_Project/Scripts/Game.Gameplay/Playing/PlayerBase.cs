using Game.Gameplay.Animations;
using Game.Events;
using UnityEngine;

namespace Game.Gameplay.Playing
{
    public abstract class PlayerBase : Entity
    {
        [SerializeField] private HealthController _healthController;
        [SerializeField] private DamageController _damageController;
        [SerializeField] private AnimationsController _animationsController;
        
        private IEventService _eventService;
        private int _index;
        
        public int Index => _index;
        public INotesExecutor NotesExecutor { get; protected set; }
        public HealthController HealthController => _healthController;
        protected IEventService EventService => _eventService;
        
        public virtual void Stop()
        {
            _healthController.Dispose();
            _damageController.Dispose();
            _animationsController.Dispose();
        }

        public virtual void Tick(float deltaTime)
        { }
        
        protected virtual void Begin(INotesExecutor notesExecutor, IEventService eventService, int index)
        {
            _eventService = eventService;
            _index = index;

            _healthController.Initialize();
            _damageController.Initialize(_healthController, _eventService, notesExecutor);
            _animationsController.Initialize();
        }
    }
}