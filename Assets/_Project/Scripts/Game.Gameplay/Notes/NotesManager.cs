using Game.Gameplay.Playing;
using System.Collections;
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
        private bool _canSetExecutor = true; //DEBUG

        public void Initialize(IEventService eventService, IPoolingService poolingService)
        {
            _eventService = eventService;
            _poolingService = poolingService;

            _notesArea.Initialize(_eventService);

            _spawnRoutine = StartCoroutine(SpawnNotesRoutine());
            
            _eventService.AddEventListener<PlayerCreatedEvent>(HandlePlayerCreated);
        }

        public void Dispose()
        {
            _eventService.RemoveEventListener<PlayerCreatedEvent>(HandlePlayerCreated);
            
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

        private void HandlePlayerCreated(ServiceEvent serviceEvent)
        {
            if (!_canSetExecutor)
            {
                return;
            }
            
            if (serviceEvent is PlayerCreatedEvent playerCreatedEvent)
            {
                Player player = playerCreatedEvent.Player;
                
                _noteExecutor = player.PlayerInputs;

                _canSetExecutor = false;////
            }
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

        private void HandleNoteExecuted(Note note)
        {
            note.Stop();

            note.OnNoteExecuted -= HandleNoteExecuted;
            
            _notesArea.RemoveNote(note);
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