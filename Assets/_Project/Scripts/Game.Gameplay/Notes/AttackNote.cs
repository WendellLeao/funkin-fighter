namespace Game.Gameplay.Notes
{
    public sealed class AttackNote : Note
    {
        public AttackNoteData AttackData => (AttackNoteData) Data;
    }
}