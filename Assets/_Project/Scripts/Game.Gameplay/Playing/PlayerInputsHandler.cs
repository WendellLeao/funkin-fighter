using Game.Gameplay.Animations;
using Game.Gameplay.Notes;
using UnityEngine;
using Game.Events;
using Game.Input;
using System;

namespace Game.Gameplay.Playing
{
    public sealed class PlayerInputsHandler : MonoBehaviour, INoteExecutor, IAnimRequester
    {
        public event Action<string> OnAnimateTrigger;
        public event Action<string, bool> OnAnimateBool;
        
        private PlayerInputsData _playerInputsData;
        private IEventService _eventService;
        private IInputService _inputService;
        private bool _mustExecuteInput;
        private Note _currentNote;

        public void Initialize(IInputService inputService, IEventService eventService)
        {
            _inputService = inputService;
            _eventService = eventService;

            _inputService.OnReadPlayerInputs += HandlePlayerInputs;
            
            _eventService.AddEventListener<NoteEnterExecuteAreaEvent>(HandleNoteEnterExecuteArea);
            _eventService.AddEventListener<NoteExitExecuteAreaEvent>(HandleNoteExitExecuteArea);
        }

        public void Dispose()
        {
            _inputService.OnReadPlayerInputs -= HandlePlayerInputs;
            
            _eventService.RemoveEventListener<NoteEnterExecuteAreaEvent>(HandleNoteEnterExecuteArea);
            _eventService.RemoveEventListener<NoteExitExecuteAreaEvent>(HandleNoteExitExecuteArea);
        }

        public void Tick(float deltaTime)
        {
            if (!CanExecuteNote(_currentNote))
            {
                return;
            }
            
            CheckNoteExecution(_currentNote);
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

            if ((PlayerInputsHandler) currentNote.NoteExecutor != this)
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