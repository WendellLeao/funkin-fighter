using Game.Gameplay.Animations;
using Game.Gameplay.Notes;
using UnityEngine;
using Game.Events;
using Game.Input;
using System;

namespace Game.Gameplay.Playing
{
    public sealed class NotesExecutor : MonoBehaviour, INotesExecutor, IAnimRequester
    {
        public event Action<Note, bool> OnNoteExecuted; 
        
        public event Action<string, bool> OnAnimateBool;
        public event Action<string> OnAnimateTrigger;
        
        private PlayerInputsData _playerInputsData;
        private IEventService _eventService;
        private IInputService _inputService;
        private bool _mustExecuteInput;
        private Note _currentNote;
        private int _index;

        public int Index => _index;

        public void Initialize(IInputService inputService, IEventService eventService, int index)
        {
            _inputService = inputService;
            _eventService = eventService;
            _index = index;

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
                Note note = noteEnterExecuteAreaEvent.Note;
                
                if (!HasAuthority(note))
                {
                    return;
                }
                
                _mustExecuteInput = true;
                
                _currentNote = note;
            }
        }
        
        private void HandleNoteExitExecuteArea(ServiceEvent serviceEvent)
        {
            if (serviceEvent is NoteEnterExecuteAreaEvent noteEnterExecuteAreaEvent)
            {
                Note note = noteEnterExecuteAreaEvent.Note;
                
                if (!HasAuthority(note))
                {
                    return;
                }

                _mustExecuteInput = false;
                        
                _currentNote = null;
            }
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
            
            OnNoteExecuted?.Invoke(note, hasCorrectlyHit);

            if (!hasCorrectlyHit)
            {
                return;
            }

            OnAnimateTrigger?.Invoke(noteData.AnimationData.ID);
        }
        
        public bool HasAuthority(Note note)
        {
            INotesExecutor notesExecutor = note.NotesExecutor;
            
            if (notesExecutor.Index == Index)
            {
                return true;
            }

            return false;
        }

        private bool CanExecuteNote(Note currentNote)
        {
            if (!_mustExecuteInput)
            {
                return false;
            }

            INotesExecutor currentNotesExecutor = currentNote.NotesExecutor;
            
            if (currentNotesExecutor.Index != Index)
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
