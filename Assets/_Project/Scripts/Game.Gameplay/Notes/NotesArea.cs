using System.Collections.Generic;
using Game.Events;
using UnityEngine;

namespace Game.Gameplay.Notes
{
    public sealed class NotesArea : MonoBehaviour
    {
        [SerializeField] private float _minimumExecutionAreaValue;
        [SerializeField] private float _maximumExecutionAreaValue;
        [SerializeField] private Transform _defendSpawnPoint;
        [SerializeField] private Transform _dodgeSpawnPoint;
        [SerializeField] private Transform _lightAttackSpawnPoint;
        [SerializeField] private Transform _heavyAttackSpawnPoint;

        private bool _noteHasEntered;
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
            for (int i = 0; i < _notes.Count; i++)
            {
                Note note = _notes[i];

                if (note == null)
                {
                    continue;
                }

                if (!IsUnderExecutionArea(note))
                {
                    if (_noteHasEntered)
                    {
                        _eventService.DispatchEvent(new NoteEnterExecuteAreaEvent(note));

                        _noteHasEntered = false;
                    }
                    
                    continue;
                }

                _eventService.DispatchEvent(new NoteEnterExecuteAreaEvent(note));

                _noteHasEntered = true;
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
    }
}