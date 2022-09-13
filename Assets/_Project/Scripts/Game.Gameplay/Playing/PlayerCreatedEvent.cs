using Game.Events;

namespace Game.Gameplay.Playing
{
    public sealed class PlayerCreatedEvent : ServiceEvent
    {
        public PlayerCreatedEvent(Player player)
        {
            Player = player;
        }
        
        public Player Player { get; }
    }
}