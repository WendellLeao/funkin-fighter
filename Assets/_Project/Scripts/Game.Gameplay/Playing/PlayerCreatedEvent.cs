using Game.Events;

namespace Game.Gameplay.Playing
{
    public sealed class PlayerCreatedEvent : ServiceEvent
    {
        public PlayerCreatedEvent(PlayerBase playerBase)
        {
            PlayerBase = playerBase;
        }
        
        public PlayerBase PlayerBase { get; }
    }
}