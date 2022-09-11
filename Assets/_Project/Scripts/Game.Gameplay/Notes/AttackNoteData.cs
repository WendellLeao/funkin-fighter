using UnityEngine;

namespace Game.Gameplay.Notes
{
    [CreateAssetMenu(menuName = "Playing/NoteData", fileName = "AttackNoteData")]
    public sealed class AttackNoteData : NoteData
    {
        [Header("Attack")]
        public int Damage;
    }
}