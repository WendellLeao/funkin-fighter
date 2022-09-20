using Game.Gameplay.Notes;
using System;

namespace Game.Gameplay.Playing
{
    public interface INotesExecutor
    {
        public event Action<Note, bool> OnNoteExecuted;
        public int Index { get; }
        public bool HasAuthority(Note note);
    }
}