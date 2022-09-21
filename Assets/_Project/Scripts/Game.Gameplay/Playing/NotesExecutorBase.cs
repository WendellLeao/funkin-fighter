using Game.Gameplay.Animations;
using Game.Gameplay.Notes;
using UnityEngine;
using Game.Events;
using Game.Input;
using System;

namespace Game.Gameplay.Playing
{
    public abstract class NotesExecutorBase : MonoBehaviour, INotesExecutor, IAnimRequester
    {
        public event Action<Note, bool> OnNoteExecuted; 
        
        public event Action<string, bool> OnAnimateBool;
        public event Action<string> OnAnimateTrigger;
        
        private IEventService _eventService;
        private IInputService _inputService;
        private bool _mustExecuteInput;
        private Note _currentNote;

        public int ExecutorID { get; private set; }
        protected Note CurrentNote => _currentNote;

        public virtual void Initialize(IEventService eventService, int playerIndex)
        {
            _eventService = eventService;
            ExecutorID = playerIndex;

            _eventService.AddEventListener<NoteEnterExecuteAreaEvent>(HandleNoteEnterExecuteArea);
            _eventService.AddEventListener<NoteExitExecuteAreaEvent>(HandleNoteExitExecuteArea);
        }

        public virtual void Dispose()
        {
            _eventService.RemoveEventListener<NoteEnterExecuteAreaEvent>(HandleNoteEnterExecuteArea);
            _eventService.RemoveEventListener<NoteExitExecuteAreaEvent>(HandleNoteExitExecuteArea);
        }

        public virtual void Tick(float deltaTime)
        {
            if (!CanExecuteNote(_currentNote))
            {
                return;
            }
            
            CheckNoteExecution(_currentNote);
        }

        protected virtual void HandleNoteEnterExecuteArea(ServiceEvent serviceEvent)
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
        
        protected virtual void HandleNoteExitExecuteArea(ServiceEvent serviceEvent)
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
        
        protected virtual void CheckNoteExecution(Note note)
        { }
        
        protected virtual void ExecuteNote(Note note, bool hasCorrectlyHit)
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
            
            if (notesExecutor.ExecutorID == ExecutorID)
            {
                return true;
            }

            return false;
        }

        protected virtual bool CanExecuteNote(Note currentNote)
        {
            if (!_mustExecuteInput)
            {
                return false;
            }

            if (currentNote.HasExecuted)
            {
                return false;
            }

            return true;
        }
    }
}