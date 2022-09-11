using System.Collections.Generic;
using Game.Events;
using UnityEngine;

namespace Game.Gameplay.Notes
{
    public sealed class NotesArea : MonoBehaviour
    {
        [SerializeField] private Transform _defendSpawnPoint;
        [SerializeField] private Transform _dodgeSpawnPoint;
        [SerializeField] private Transform _lightAttackSpawnPoint;
        [SerializeField] private Transform _heavyAttackSpawnPoint;
        [SerializeField] private float _minimumExecutionAreaValue;
        [SerializeField] private float _maximumExecutionAreaValue;

        private IEventService _eventService;
        private List<Note> _notes = new List<Note>();

        public Vector3 DefendSpawnPoint => _defendSpawnPoint.position;
        public Vector3 DodgeSpawnPoint => _dodgeSpawnPoint.position;
        public Vector3 LightAttackSpawnPoint => _lightAttackSpawnPoint.position;
        public Vector3 HeavyAttackSpawnPoint => _heavyAttackSpawnPoint.position;

        public void Initialize(IEventService eventService)
        {
            _eventService = eventService;
        }

        public void Tick(float deltaTime)
        {
            foreach (Note note in _notes)
            {
                if (IsUnderExecutionArea(note))
                {
                    if (note.HasEnteredExecutionArea)
                    {
                        continue;
                    }
                    
                    note.EnterExecutionArea();

                    _eventService.DispatchEvent(new NoteEnterExecuteAreaEvent(note));
                    
                    // Debug.Log("<color=green>Has entered area</color>");
                    
                    continue;
                }
                
                if (note.HasEnteredExecutionArea)
                {
                    note.ExitExecutionArea();
                        
                    _eventService.DispatchEvent(new NoteExitExecuteAreaEvent(note));
                        
                    // Debug.Log("<color=red>Has exit area</color>");
                }
            }
        }

        private bool IsUnderExecutionArea(Note note)
        {
            Vector2 anchoredPosition = note.AnchoredPosition;
            
            return anchoredPosition.y <= _minimumExecutionAreaValue && anchoredPosition.y >= _maximumExecutionAreaValue;
        }

        public void AddNote(Note note)
        {
            _notes.Add(note);
        }

        public void RemoveNote(Note note)
        {
            _notes.Remove(note);
        }
    }
}