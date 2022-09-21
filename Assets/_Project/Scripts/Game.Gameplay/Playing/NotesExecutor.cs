using Game.Gameplay.Notes;
using Game.Services;
using Game.Events;
using Game.Input;

namespace Game.Gameplay.Playing
{
    public sealed class NotesExecutor : NotesExecutorBase
    {
        private PlayerInputsData _playerInputsData;
        private IInputService _inputService;

        public override void Initialize(IEventService eventService, int playerIndex)
        {
            base.Initialize(eventService, playerIndex);

            _inputService = ServiceLocator.GetService<IInputService>();

            _inputService.OnReadPlayerInputs += HandlePlayerInputs;
        }

        public override void Dispose()
        {
            base.Dispose();

            _inputService.OnReadPlayerInputs -= HandlePlayerInputs;
        }

        private void HandlePlayerInputs(PlayerInputsData playerInputs)
        {
            _playerInputsData = playerInputs;
        }

        protected override void CheckNoteExecution(Note note)
        {
            base.CheckNoteExecution(note);
            
            switch (note.Data.Type)
            {
                case NoteType.LightAttack:
                {
                    ExecuteNote(note, _playerInputsData.ExecuteLightAttack);

                    break;
                }
                case NoteType.HeavyAttack:
                {
                    ExecuteNote(note, _playerInputsData.ExecuteHeavyAttack);
                    
                    break;
                }
                case NoteType.Defend:
                {
                    ExecuteNote(note, _playerInputsData.ExecuteDefend);
                    
                    break;
                }
                case NoteType.Dodge:
                {
                    ExecuteNote(note, _playerInputsData.ExecuteDodge);
                    
                    break;
                }
            }
        }
        
        protected override bool CanExecuteNote(Note currentNote)
        {
            base.CanExecuteNote(currentNote);
            
            if (!HasExecutedInput())
            {
                return false;
            }

            return true;
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
