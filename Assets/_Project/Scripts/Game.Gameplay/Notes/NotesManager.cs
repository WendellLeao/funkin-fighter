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
        private Coroutine _spawnRoutine;
        private int _lastRandomIndex;

        public void Initialize()
        {
            _poolingService = ServiceLocator.GetService<IPoolingService>();
            _eventService = ServiceLocator.GetService<IEventService>();
            
            _spawnRoutine = StartCoroutine(SpawnNotesRoutine());
            
            _notesArea.Initialize(_eventService);
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

            _notesArea.AddNote(note);
            
            CheckAndSetNotePosition(note);
            
            note.Begin();
            
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
    }
}