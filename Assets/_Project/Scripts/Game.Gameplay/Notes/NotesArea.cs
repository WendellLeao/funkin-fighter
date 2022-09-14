using System.Collections.Generic;
using Game.Gameplay.Playing;
using System.Collections;
using Game.Pooling;
using Game.Events;
using UnityEngine;

namespace Game.Gameplay.Notes
{
    public sealed class NotesArea : MonoBehaviour
    {
        [SerializeField] private Note[] _notes;
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private float _spawnNotesRate;
        
        [Header("Spawn Points")]
        [SerializeField] private Transform _defendSpawnPoint;
        [SerializeField] private Transform _dodgeSpawnPoint;
        [SerializeField] private Transform _lightAttackSpawnPoint;
        [SerializeField] private Transform _heavyAttackSpawnPoint;
        
        [Header("Execution Area")]
        [SerializeField] private float _minimumExecutionAreaValue;
        [SerializeField] private float _maximumExecutionAreaValue;

        private IPoolingService _poolingService;
        private IEventService _eventService;
        private INoteExecutor _noteExecutor;
        private List<Note> _activeNotes;
        private Coroutine _spawnRoutine;
        private int _lastRandomIndex;

        public void Begin(INoteExecutor noteExecutor, IEventService eventService)
        {
            _noteExecutor = noteExecutor;
            _eventService = eventService;

            _activeNotes = new List<Note>();
            
            _spawnRoutine = StartCoroutine(SpawnNotesRoutine());

            _noteExecutor.OnInputExecuted += HandleInputExecuted;
        }

        public void Stop()
        {
            _noteExecutor.OnInputExecuted -= HandleInputExecuted;
            
            if (_spawnRoutine != null)
            {
                StopCoroutine(_spawnRoutine);
            }
        }
        
        public void Tick(float deltaTime)
        {
            if (_activeNotes.Count <= 0)
            {
                return;
            }

            RefreshExecutionArea();
        }

        private void HandleInputExecuted(Note note, bool hasCorrectlyHit)
        {
            _eventService.DispatchEvent(new InputExecutedEvent(_noteExecutor, note, hasCorrectlyHit));
        }
        
        private IEnumerator SpawnNotesRoutine()
        {
            Note note = SpawnRandomNote();

            note.SetExecutor(_noteExecutor);
            
            note.OnNoteExecuted += HandleNoteExecuted;
            
            _activeNotes.Add(note);
            
            CheckAndSetNotePosition(note);
            
            note.Begin(_poolingService);
            
            yield return new WaitForSeconds(_spawnNotesRate);
            
            _spawnRoutine = StartCoroutine(SpawnNotesRoutine());
        }
        
        private void HandleNoteExecuted(Note note)
        {
            note.Stop();

            note.OnNoteExecuted -= HandleNoteExecuted;
            
            _activeNotes.Remove(note);
        }
        
        private Note SpawnRandomNote()
        {
            bool hasFoundUniqueNumber = false;
            
            while (hasFoundUniqueNumber == false)
            {
                int randomIndex = Random.Range(0, _notes.Length);

                if (randomIndex != _lastRandomIndex)
                {
                    _lastRandomIndex = randomIndex;

                    hasFoundUniqueNumber = true;
                }
            }

            Note newNote = Instantiate(_notes[_lastRandomIndex], transform);//TODO: Use pooling

            return newNote;
        }
        
        private void CheckAndSetNotePosition(Note note)
        {
            Vector3 targetPosition = Vector3.zero;
            
            NoteData noteData = note.Data;
            
            switch (noteData.Type)
            {
                case NoteType.Defend:
                {
                    targetPosition = _defendSpawnPoint.position;
                    
                    break;
                }
                case NoteType.Dodge:
                {
                    targetPosition = _dodgeSpawnPoint.position;
                    
                    break;
                }
                case NoteType.LightAttack:
                {
                    targetPosition = _lightAttackSpawnPoint.position;
                    
                    break;
                }
                case NoteType.HeavyAttack:
                {
                    targetPosition = _heavyAttackSpawnPoint.position;
                    
                    break;
                }
            } 
            
            note.SetPosition(targetPosition);
        }

        private void RefreshExecutionArea()
        {
            foreach (Note note in _activeNotes)
            {
                if (NoteHasEnteredExecutionArea(note))
                {
                    if (note.HasEnteredExecutionArea)
                    {
                        continue;
                    }
                    
                    note.EnterExecutionArea();

                    _eventService.DispatchEvent(new NoteEnterExecuteAreaEvent(note));
                    
                    continue;
                }
                
                if (note.HasEnteredExecutionArea)
                {
                    note.ExitExecutionArea();
                        
                    _eventService.DispatchEvent(new NoteExitExecuteAreaEvent(note));
                }
            }
        }

        private bool NoteHasEnteredExecutionArea(Note note)
        {
            Vector2 anchoredPosition = note.AnchoredPosition;
            
            return anchoredPosition.y <= _minimumExecutionAreaValue && anchoredPosition.y >= _maximumExecutionAreaValue;
        }
    }
}