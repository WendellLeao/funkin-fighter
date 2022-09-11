using Game.Gameplay.Notes;
using UnityEngine;
using Game.Events;

namespace Game.Gameplay.Playing
{
    public sealed class DamageController : MonoBehaviour
    {
        private HealthController _healthController;
        private IEventService _eventService;
        private INoteExecutor _localExecutor;

        public void Initialize(INoteExecutor executor, IEventService eventService, HealthController healthController)
        {
            _healthController = healthController;
            _eventService = eventService;
            _localExecutor = executor;
            
            _eventService.AddEventListener<InputExecutedEvent>(HandleInputExecutedEvent);
        }

        public void Dispose()
        {
            _eventService.RemoveEventListener<InputExecutedEvent>(HandleInputExecutedEvent);
        }
        
        private void HandleInputExecutedEvent(ServiceEvent serviceEvent)
        {
            if (serviceEvent is InputExecutedEvent inputExecutedEvent)
            {
                if (!inputExecutedEvent.HasCorrectlyHit)
                {
                    return;
                }

                Note note = inputExecutedEvent.Note;
                
                INoteExecutor noteExecutor = inputExecutedEvent.Executor;
                
                if (note is AttackNote attackNote && !IsLocalExecutor(noteExecutor))
                {
                    //TODO: Apply damage
                    
                    return;
                }

                if (note is DefenseNote defenseNote && IsLocalExecutor(noteExecutor))
                {
                    //TODO: Add temporary shield
                }
            }
        }

        private bool IsLocalExecutor(INoteExecutor executor)
        {
            if (executor == _localExecutor)
            {
                return true;
            }

            return false;
        }
    }
}