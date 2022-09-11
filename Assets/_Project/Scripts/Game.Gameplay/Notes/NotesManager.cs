using Game.Gameplay.Playing;
using System.Collections;
using Game.Services;
using Game.Pooling;
using Game.Events;
using UnityEngine;

namespace Game.Gameplay.Notes
{
    public sealed class NotesManager : MonoBehaviour
    {
        [SerializeField] private Note[] _notes;
        [SerializeField] private NotesArea _notesArea;
        [SerializeField] private float _spawnNotesRate;

        private IPoolingService _poolingService;
        private IEventService _eventService;
        private INoteExecutor _noteExecutor;
        private Coroutine _spawnRoutine;
        private int _lastRandomIndex;

        public void Initialize(INoteExecutor noteExecutor)
        {
            _noteExecutor = noteExecutor;
            
            _poolingService = ServiceLocator.GetService<IPoolingService>();
            _eventService = ServiceLocator.GetService<IEventService>();
            
            _notesArea.Initialize(_eventService);

            _spawnRoutine = StartCoroutine(SpawnNotesRoutine());
        }

        public void Dispose()
        {
            if (_spawnRoutine == null)
            {
                return;
            }
            
            StopCoroutine(_spawnRoutine);
        }

        public void Tick(float deltaTime)
        {
            _notesArea.Tick(deltaTime);
        }

        private IEnumerator SpawnNotesRoutine()
        {
            Note note = SpawnRandomNote();

            note.SetExecutor(_noteExecutor);
            
            note.OnNoteExecuted += HandleNoteExecuted;
            
            _notesArea.AddNote(note);
            
            CheckAndSetNotePosition(note);
            
            note.Begin(_poolingService);
            
            yield return new WaitForSeconds(_spawnNotesRate);
            
            _spawnRoutine = StartCoroutine(SpawnNotesRoutine());
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

            Note newNote = Instantiate(_notes[_lastRandomIndex], _notesArea.transform);//TODO: Use pooling

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
                    targetPosition = _notesArea.DefendSpawnPoint;
                    
                    break;
                }
                case NoteType.Dodge:
                {
                    targetPosition = _notesArea.DodgeSpawnPoint;
                    
                    break;
                }
                case NoteType.LightAttack:
                {
                    targetPosition = _notesArea.LightAttackSpawnPoint;
                    
                    break;
                }
                case NoteType.HeavyAttack:
                {
                    targetPosition = _notesArea.HeavyAttackSpawnPoint;
                    
                    break;
                }
            } 
            
            note.SetPosition(targetPosition);
        }

        private void HandleNoteExecuted(Note note)
        {
            note.Stop();

            note.OnNoteExecuted -= HandleNoteExecuted;
            
            _notesArea.RemoveNote(note);
        }
    }
}