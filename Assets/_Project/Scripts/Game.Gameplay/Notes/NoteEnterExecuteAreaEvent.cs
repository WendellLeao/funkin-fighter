using Game.Events;

namespace Game.Gameplay.Notes
{
    public sealed class NoteEnterExecuteAreaEvent : ServiceEvent
    {
        public NoteEnterExecuteAreaEvent(Note note)
        {
            Note = note;
        }
        
        public Note Note { get; }
    }
}