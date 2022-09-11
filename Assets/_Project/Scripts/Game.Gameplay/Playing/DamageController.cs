using Game.Gameplay.Notes;
using UnityEngine;
using Game.Events;
using System;

namespace Game.Gameplay.Playing
{
    public sealed class DamageController : MonoBehaviour
    {
        private HealthController _healthController;
        private IEventService _eventService;
        private INoteExecutor _localExecutor;
        private int _damageAbsorption;
        private bool _isInvencible;
        
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
                    if (_isInvencible)
                    {
                        return;
                    }
                    
                    ApplyDamage(attackNote);

                    _damageAbsorption = 0;
                    
                    _isInvencible = false;

                    return;
                }

                if (note is DefenseNote defenseNote && IsLocalExecutor(noteExecutor))
                {
                    DefenseNoteData defenseData = defenseNote.DefenseData;

                    _damageAbsorption = defenseData.DamageAbsorption;

                    _isInvencible = defenseData.IsInvencible;
                }
            }
        }

        private void ApplyDamage(AttackNote attackNote)
        {
            AttackNoteData attackData = attackNote.AttackData;

            float damage = attackData.Damage;

            damage -= _damageAbsorption; //TODO: Review this

            damage = Math.Clamp(damage, 0, attackData.Damage);

            _healthController.Remove(damage);
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