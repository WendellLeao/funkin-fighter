using Random = UnityEngine.Random;
using Game.Gameplay.Animations;
using Game.Gameplay.Notes;
using UnityEngine;
using Game.Events;
using System;

namespace Game.Gameplay.Playing.Automatic
{
    public sealed class AutomaticNotesExecutor : MonoBehaviour, INotesExecutor, IAnimRequester
    {
        public event Action<Note, bool> OnNoteExecuted; 
        
        public event Action<string, bool> OnAnimateBool;
        public event Action<string> OnAnimateTrigger;
        
        [Header("Chances")]
        [SerializeField, Range(0f, 1f)] private float _lightAttackChance = 0.8f;
        [SerializeField, Range(0f, 1f)] private float _heavyAttackChance = 0.75f;
        [SerializeField, Range(0f, 1f)] private float _defendChance = 0.7f;
        [SerializeField, Range(0f, 1f)] private float _dodgeChance = 0.8f;
        
        private IEventService _eventService;
        private bool _mustExecuteInput;
        private Note _currentNote;
        private int _index;

        public int Index => _index;

        public void Initialize(IEventService eventService, int index)
        {
            _eventService = eventService;
            _index = index;

            _eventService.AddEventListener<NoteEnterExecuteAreaEvent>(HandleNoteEnterExecuteArea);
            _eventService.AddEventListener<NoteExitExecuteAreaEvent>(HandleNoteExitExecuteArea);
        }

        public void Dispose()
        {
            _eventService.RemoveEventListener<NoteEnterExecuteAreaEvent>(HandleNoteEnterExecuteArea);
            _eventService.RemoveEventListener<NoteExitExecuteAreaEvent>(HandleNoteExitExecuteArea);
        }

        public void Tick(float deltaTime)
        {
            if (!CanExecuteNote(_currentNote))
            {
                return;
            }

            bool hasCorrectlyHit = HasCorrectlyHit();
            
            ExecuteNote(_currentNote, hasCorrectlyHit);
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
                
                _currentNote = noteEnterExecuteAreaEvent.Note;
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
        
        private void ExecuteNote(Note note, bool hasCorrectlyHit)
        {
            NoteData noteData = note.Data;
            
            note.Execute(hasCorrectlyHit);
            
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

            if (currentNote.HasExecuted)
            {
                return false;
            }

            return true;
        }

        private float GetChanceToHit(Note note)
        {
            float chance = 0f;

            NoteData noteData = note.Data;
            
            switch (noteData.Type)
            {
                case NoteType.Defend:
                {
                    chance = _defendChance;
                    
                    break;
                }
                case NoteType.Dodge:
                {
                    chance = _dodgeChance;
                    
                    break;
                }
                case NoteType.LightAttack:
                {
                    chance = _lightAttackChance;
                    
                    break;
                }
                case NoteType.HeavyAttack:
                {
                    chance = _heavyAttackChance;
                    
                    break;
                }
            }

            return chance;
        }

        private bool HasCorrectlyHit()
        {
            return Random.value < GetChanceToHit(_currentNote);
        }
    }
}