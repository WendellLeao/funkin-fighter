using System;

namespace Game.Gameplay.Playing
{
    public interface IAnimRequester
    {
        public event Action<string> OnAnimateTrigger;
        public event Action<string, bool> OnAnimateBool;
    }
}