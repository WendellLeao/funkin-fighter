using Game.Gameplay.Notes;
using Game.Events;

namespace Game.Gameplay.Playing
{
    public sealed class NoteExecutedEvent : ServiceEvent
    {
        public NoteExecutedEvent(INotesExecutor executor, Note note, bool hasCorrectlyHit)
        {
            Note = note;
            Executor = executor;
            HasCorrectlyHit = hasCorrectlyHit;
        }
        
        public Note Note { get; }
        public INotesExecutor Executor { get; }
        public bool HasCorrectlyHit { get; }
    }
}