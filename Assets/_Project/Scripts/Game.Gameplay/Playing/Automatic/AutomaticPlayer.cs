using Game.Events;
using UnityEngine;

namespace Game.Gameplay.Playing.Automatic
{
    public sealed class AutomaticPlayer : PlayerBase
    {
        [SerializeField] private AutomaticNotesExecutor _notesExecutor;
        
        public override void Stop()
        {
            base.Stop();
            
            _notesExecutor.Dispose();
        }

        public override void Tick(float deltaTime)
        {
            base.Tick(deltaTime);
            
            _notesExecutor.Tick(deltaTime);
        }
        
        public void Begin(IEventService eventService, int index)
        {
            base.Begin(_notesExecutor, eventService, index);
            
            NotesExecutor = _notesExecutor;

            _notesExecutor.Initialize(EventService, index);
        }
    }
}
