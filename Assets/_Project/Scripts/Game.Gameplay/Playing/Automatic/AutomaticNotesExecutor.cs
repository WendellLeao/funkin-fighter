using Random = UnityEngine.Random;
using Game.Gameplay.Notes;
using UnityEngine;

namespace Game.Gameplay.Playing.Automatic
{
    public sealed class AutomaticNotesExecutor : NotesExecutorBase
    {
        [Header("Chances")]
        [SerializeField, Range(0f, 1f)] private float _lightAttackChance = 0.8f;
        [SerializeField, Range(0f, 1f)] private float _heavyAttackChance = 0.75f;
        [SerializeField, Range(0f, 1f)] private float _defendChance = 0.7f;
        [SerializeField, Range(0f, 1f)] private float _dodgeChance = 0.8f;
        
        public override void Tick(float deltaTime)
        {
            base.Tick(deltaTime);

            if (!CanExecuteNote(CurrentNote))
            {
                return;
            }
            
            bool hasCorrectlyHit = HasCorrectlyHit();
            
            ExecuteNote(CurrentNote, hasCorrectlyHit);
        }

        private float GetChanceToHit(Note note)
        {
            float chance = 0f;

            NoteData noteData = note.Data;
            
            switch (noteData.Type)
            {
                case NoteType.Defend:
                {
                    chance = _defendChance;
                    
                    break;
                }
                case NoteType.Dodge:
                {
                    chance = _dodgeChance;
                    
                    break;
                }
                case NoteType.LightAttack:
                {
                    chance = _lightAttackChance;
                    
                    break;
                }
                case NoteType.HeavyAttack:
                {
                    chance = _heavyAttackChance;
                    
                    break;
                }
            }

            return chance;
        }

        private bool HasCorrectlyHit()
        {
            return Random.value < GetChanceToHit(CurrentNote);
        }
    }
}