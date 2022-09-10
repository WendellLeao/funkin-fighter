using Game.Events;

namespace Game.Gameplay.Notes
{
    public sealed class NoteExitExecuteAreaEvent : ServiceEvent
    {
        public NoteExitExecuteAreaEvent(Note note)
        {
            Note = note;
        }
        
        public Note Note { get; }
    }
}