using Game.Gameplay.Notes;
using System;

namespace Game.Gameplay.Playing
{
    public interface INoteExecutor
    {
        public event Action<Note, bool> OnInputExecuted;
    }
}