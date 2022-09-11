using Game.Gameplay.Notes;
using UnityEngine;
using Game.Events;
using Game.Input;

namespace Game.Gameplay.Playing
{
    public sealed class Player : Entity, INoteExecutor
    {
        [Header("Controllers")]
        [SerializeField] private HealthController _healthController;
        [SerializeField] private DamageController _damageController;

        private PlayerInputsData _playerInputsData;
        private IEventService _eventService;
        private IInputService _inputService;
        private bool _mustExecuteInput;
        private Note _currentNote;

        public void Begin(IEventService eventService, IInputService inputService)
        {
            _eventService = eventService;
            _inputService = inputService;

            _healthController.Initialize();
            _damageController.Initialize(this, _eventService, _healthController);

            SubscribeEvents();
        }

        public void Stop()
        {
            _healthController.Dispose();
            _damageController.Dispose();
            
            UnsubscribeEvents();
        }

        public void Tick(float deltaTime)
        {
            if (!_mustExecuteInput)
            {
                return;
            }

            if (!HasExecutedInput())
            {
                return;
            }

            if (_currentNote.HasExecuted)
            {
                return;
            }
            
            CheckNoteExecution(_currentNote);
        }

        private void SubscribeEvents()
        {
            _inputService.OnReadPlayerInputs += HandlePlayerInputs;
            
            _eventService.AddEventListener<NoteEnterExecuteAreaEvent>(HandleNoteEnterExecuteArea);
            _eventService.AddEventListener<NoteExitExecuteAreaEvent>(HandleNoteExitExecuteArea);
        }
        
        private void UnsubscribeEvents()
        {
            _inputService.OnReadPlayerInputs -= HandlePlayerInputs;
            
            _eventService.RemoveEventListener<NoteEnterExecuteAreaEvent>(HandleNoteEnterExecuteArea);
            _eventService.RemoveEventListener<NoteExitExecuteAreaEvent>(HandleNoteExitExecuteArea);
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
            if (serviceEvent is NoteExitExecuteAreaEvent noteExitExecuteAreaEvent)
            {
                Note note = noteExitExecuteAreaEvent.Note;
                
                if (!note.HasExecuted)
                {
                    Debug.Log("<color=red>Perdeu a nota</color>");
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

                    if (_playerInputsData.ExecuteLightAttack)
                    {
                        Debug.Log($"<color=green>Acertou a nota: {NoteType.LightAttack}</color>");
                    }
                    
                    break;
                }
                case NoteType.HeavyAttack:
                {
                    HandleInputExecution(note, _playerInputsData.ExecuteHeavyAttack);

                    if (_playerInputsData.ExecuteHeavyAttack)
                    {
                        Debug.Log($"<color=green>Acertou a nota: {NoteType.HeavyAttack}</color>");
                    }
                    
                    break;
                }
                case NoteType.Defend:
                {
                    HandleInputExecution(note, _playerInputsData.ExecuteDefend);

                    if (_playerInputsData.ExecuteDefend)
                    {
                        Debug.Log($"<color=green>Acertou a nota: {NoteType.Defend}</color>");
                    }
                    
                    break;
                }
                case NoteType.Dodge:
                {
                    HandleInputExecution(note, _playerInputsData.ExecuteDodge);

                    if (_playerInputsData.ExecuteDodge)
                    {
                        Debug.Log($"<color=green>Acertou a nota: {NoteType.Dodge}</color>");
                    }
                    
                    break;
                }
            }
        }
        
        private void HandleInputExecution(Note note, bool hasCorrectlyHit)
        {
            note.Execute(hasCorrectlyHit);
            
            _eventService.DispatchEvent(new InputExecutedEvent(this, note, hasCorrectlyHit));
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
