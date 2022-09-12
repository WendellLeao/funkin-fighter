using UnityEngine;

namespace Game.Gameplay.Notes
{
    [CreateAssetMenu(menuName = "Playing/NoteData", fileName = "DefenseNoteData")]
    public sealed class DefenseNoteData : NoteData
    {
        [Header("Defense"), Range(0f, 100f)]
        public int DamageAbsorption;
        public bool MustIgnoreDamage;
    }
}