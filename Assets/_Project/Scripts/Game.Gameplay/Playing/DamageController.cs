using Game.Gameplay.Animations;
using Game.Gameplay.Notes;
using UnityEngine;
using Game.Events;
using System;

namespace Game.Gameplay.Playing
{
    public sealed class DamageController : MonoBehaviour, IAnimRequester
    {
        public event Action<string> OnAnimateTrigger;
        public event Action<string, bool> OnAnimateBool;
        
        private HealthController _healthController;
        private INotesExecutor _notesExecutor;
        private IEventService _eventService;
        private bool _mustIgnoreDamage;
        private int _damageAbsorption;
        
        public void Initialize(HealthController healthController, IEventService eventService, INotesExecutor executor)
        {
            _healthController = healthController;
            _eventService = eventService;
            _notesExecutor = executor;
            
            _eventService.AddEventListener<NoteExecutedEvent>(HandleInputExecutedEvent);
        }

        public void Dispose()
        {
            _eventService.RemoveEventListener<NoteExecutedEvent>(HandleInputExecutedEvent);
        }
        
        private void HandleInputExecutedEvent(ServiceEvent serviceEvent)
        {
            if (serviceEvent is NoteExecutedEvent inputExecutedEvent)
            {
                if (!inputExecutedEvent.HasCorrectlyHit)
                {
                    return;
                }

                Note note = inputExecutedEvent.Note;
                
                INotesExecutor notesExecutor = inputExecutedEvent.Executor;
                
                if (note is AttackNote attackNote)
                {
                    if (_mustIgnoreDamage || IsNoteExecutor(notesExecutor))
                    {
                        return;
                    }
                    
                    ApplyDamage(attackNote);
                    
                    OnAnimateTrigger?.Invoke("Hit");//TODO: Set the animation id dynamically

                    return;
                }

                if (!IsNoteExecutor(notesExecutor))
                {
                    return;
                }
                
                DefenseNote defenseNote = (DefenseNote) note;

                AbsorbDamage(defenseNote);
            }
        }

        private void ApplyDamage(AttackNote attackNote)
        {
            AttackNoteData attackData = attackNote.AttackData;

            float damage = CalculateDamage(attackData);

            _healthController.Remove(damage);

            RemoveDamageAbsorption();
        }

        private float CalculateDamage(AttackNoteData attackData)
        {
            float damage = attackData.Damage;

            damage -= _damageAbsorption;//TODO: Review this

            damage = Math.Clamp(damage, 0, attackData.Damage);
            
            return damage;
        }

        private void AbsorbDamage(DefenseNote defenseNote)
        {
            DefenseNoteData defenseData = defenseNote.DefenseData;

            _damageAbsorption = defenseData.DamageAbsorption;

            _mustIgnoreDamage = defenseData.MustIgnoreDamage;
                    
            OnAnimateTrigger?.Invoke(defenseData.AnimationData.ID);
        }
        
        private void RemoveDamageAbsorption()
        {
            _damageAbsorption = 0;
                    
            _mustIgnoreDamage = false;
        }
        
        private bool IsNoteExecutor(INotesExecutor executor)
        {
            return executor == _notesExecutor;
        }
    }
}