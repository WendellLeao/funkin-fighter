using Game.Events;

namespace Game.Gameplay.Playing
{
    public sealed class InputExecutionEvent : ServiceEvent
    {
        public InputExecutionEvent(bool hasCorrectlyHit)
        {
            HasCorrectlyHit = hasCorrectlyHit;
        }
        
        public bool HasCorrectlyHit { get; }
    }
}