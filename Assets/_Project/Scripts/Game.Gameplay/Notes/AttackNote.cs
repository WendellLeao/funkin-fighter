using UnityEngine;

namespace Game.Gameplay.Notes
{
    public sealed class AttackNote : Note
    {
        protected override void Execute()
        {
            switch (Data.Type)
            {
                case NoteType.LightAttack:
                {
                    Debug.Log("<color=cyan>Execute Light Attack</color>");
                    
                    break;
                }
                case NoteType.HeavyAttack:
                {
                    Debug.Log("<color=cyan>Execute Heavy Attack</color>");
                    
                    break;
                }
            }
        }
    }
}