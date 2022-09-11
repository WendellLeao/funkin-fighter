using Game.Gameplay.Notes;
using Game.Events;

namespace Game.Gameplay.Playing
{
    public sealed class InputExecutedEvent : ServiceEvent
    {
        public InputExecutedEvent(INoteExecutor executor, Note note, bool hasCorrectlyHit)
        {
            Note = note;
            Executor = executor;
            HasCorrectlyHit = hasCorrectlyHit;
        }
        
        public Note Note { get; }
        public INoteExecutor Executor { get; }
        public bool HasCorrectlyHit { get; }
    }
}