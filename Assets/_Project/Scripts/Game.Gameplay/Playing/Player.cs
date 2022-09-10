using Game.Gameplay.Notes;
using UnityEngine;
using Game.Events;
using Game.Input;

namespace Game.Gameplay.Playing
{
    public sealed class Player : Entity
    {
        private IEventService _eventService;
        private IInputService _inputService;
        private PlayerInputsData _playerInputsData;
        private bool _mustExecuteInput;
        private bool _hasExecuteInput;
        private Note _currentNote;

        public void Begin(IEventService eventService, IInputService inputService)
        {
            _eventService = eventService;
            _inputService = inputService;
            
            _inputService.OnReadPlayerInputs += HandlePlayerInputs;
            
            _eventService.AddEventListener<NoteEnterExecuteAreaEvent>(HandleNoteEnterExecuteArea);
            _eventService.AddEventListener<NoteExitExecuteAreaEvent>(HandleNoteExitExecuteArea);
        }

        public void Stop()
        {
            _inputService.OnReadPlayerInputs -= HandlePlayerInputs;
            
            _eventService.RemoveEventListener<NoteEnterExecuteAreaEvent>(HandleNoteEnterExecuteArea);
            _eventService.RemoveEventListener<NoteExitExecuteAreaEvent>(HandleNoteExitExecuteArea);
        }

        public void Tick(float deltaTime)
        {
            if (!_mustExecuteInput)
            {
                return;
            }
            
            switch (_currentNote.Data.Type)
            {
                case NoteType.LightAttack:
                {
                    HandleInputExecution(_playerInputsData.ExecuteLightAttack);

                    break;
                }
                case NoteType.HeavyAttack:
                {
                    HandleInputExecution(_playerInputsData.ExecuteHeavyAttack);

                    break;
                }
                case NoteType.Defend:
                {
                    HandleInputExecution(_playerInputsData.ExecuteDefend);

                    break;
                }
                case NoteType.Dodge:
                {
                    HandleInputExecution(_playerInputsData.ExecuteDodge);

                    break;
                }
            }
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
                if (!_hasExecuteInput)
                {
                    Debug.Log("<color=red>ERROU a nota</color>");
                }
                
                _mustExecuteInput = false;
                    
                _hasExecuteInput = false;

                _currentNote = null;
            }
        }

        private void HandleInputExecution(bool hasCorrectlyHit)
        {
            _hasExecuteInput = true;
            
            _eventService.DispatchEvent(new InputExecutionEvent(hasCorrectlyHit));
            
            if (hasCorrectlyHit)
            {
                Debug.Log("<color=green>Acertou a nota</color>");
                
                return;
            }
        
            Debug.Log("<color=red>ERROU a nota</color>");
        }
    }
}
