using Game.Gameplay.Notes;
using UnityEngine;
using Game.Events;
using Game.Input;
using System;

namespace Game.Gameplay.Playing
{
    public sealed class Player : Entity, INoteExecutor, IAnimRequester
    {
        public event Action<string> OnAnimateTrigger;
        public event Action<string, bool> OnAnimateBool;
        
        [Header("Controllers")]
        [SerializeField] private HealthController _healthController;
        [SerializeField] private DamageController _damageController;
        [SerializeField] private AnimationsController _animationsController;

        private PlayerInputsData _playerInputsData;
        private IEventService _eventService;
        private IInputService _inputService;
        private bool _mustExecuteInput;
        private Note _currentNote;

        public HealthController HealthController => _healthController;

        public void Begin(IEventService eventService, IInputService inputService)
        {
            _eventService = eventService;
            _inputService = inputService;

            _healthController.Initialize();
            _damageController.Initialize(this, _eventService, _healthController);
            _animationsController.Initialize();

            SubscribeEvents();
        }

        public void Stop()
        {
            _healthController.Dispose();
            _damageController.Dispose();
            _animationsController.Dispose();
            
            UnsubscribeEvents();
        }

        public void Tick(float deltaTime)
        {
            if (!CanExecuteNote(_currentNote))
            {
                return;
            }
            
            CheckNoteExecution(_currentNote);
        }

        private void SubscribeEvents()
        {
            _inputService.OnReadPlayerInputs += HandlePlayerInputs;
            
            _eventService.AddEventListener<NoteEnterExecuteAreaEvent>(HandleNoteEnterExecuteArea);
            _eventService.AddEventListener<NoteExitExecuteAreaEvent>(HandleNoteExitExecuteArea);
        }
        
        private void UnsubscribeEvents()
        {
            _inputService.OnReadPlayerInputs -= HandlePlayerInputs;
            
            _eventService.RemoveEventListener<NoteEnterExecuteAreaEvent>(HandleNoteEnterExecuteArea);
            _eventService.RemoveEventListener<NoteExitExecuteAreaEvent>(HandleNoteExitExecuteArea);
        }
        
        private void HandlePlayerInputs(PlayerInputsData playerInputs)
        {
            _playerInputsData = playerInputs;
        }

        private void HandleNoteEnterExecuteArea(ServiceEvent serviceEvent)
        {
            if (serviceEvent is NoteEnterExecuteAreaEvent noteEnterExecuteAreaEvent)
            {
                _mustExecuteInput = true;
                
                _currentNote = noteEnterExecuteAreaEvent.Note;
            }
        }
        
        private void HandleNoteExitExecuteArea(ServiceEvent serviceEvent)
        {
            _mustExecuteInput = false;
                    
            _currentNote = null;
        }
        
        private void CheckNoteExecution(Note note)
        {
            switch (note.Data.Type)
            {
                case NoteType.LightAttack:
                {
                    HandleInputExecution(note, _playerInputsData.ExecuteLightAttack);

                    break;
                }
                case NoteType.HeavyAttack:
                {
                    HandleInputExecution(note, _playerInputsData.ExecuteHeavyAttack);
                    
                    break;
                }
                case NoteType.Defend:
                {
                    HandleInputExecution(note, _playerInputsData.ExecuteDefend);
                    
                    break;
                }
                case NoteType.Dodge:
                {
                    HandleInputExecution(note, _playerInputsData.ExecuteDodge);
                    
                    break;
                }
            }
        }
        
        private void HandleInputExecution(Note note, bool hasCorrectlyHit)
        {
            note.Execute(hasCorrectlyHit);

            NoteData noteData = note.Data;
            
            OnAnimateTrigger?.Invoke(noteData.AnimationData.ID);
            
            _eventService.DispatchEvent(new InputExecutedEvent(this, note, hasCorrectlyHit));
        }

        private bool CanExecuteNote(Note currentNote)
        {
            if (!_mustExecuteInput)
            {
                return false;
            }

            Player noteExecutor = (Player) currentNote.NoteExecutor;
            
            if (noteExecutor != this)
            {
                return false;
            }

            if (!HasExecutedInput())
            {
                return false;
            }

            if (currentNote.HasExecuted)
            {
                return false;
            }

            return true;
        }
        
        private bool HasExecutedInput()
        {
            if (_playerInputsData.ExecuteLightAttack)
            {
                return true;
            }
            
            if (_playerInputsData.ExecuteHeavyAttack)
            {
                return true;
            }
            
            if (_playerInputsData.ExecuteDefend)
            {
                return true;
            }
            
            if (_playerInputsData.ExecuteDodge)
            {
                return true;
            }

            return false;
        }
    }
}
