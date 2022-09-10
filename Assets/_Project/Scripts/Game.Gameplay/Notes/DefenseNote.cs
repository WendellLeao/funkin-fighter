using UnityEngine;

namespace Game.Gameplay.Notes
{
    public sealed class DefenseNote : Note
    {
        protected override void Execute()
        {
            switch (Data.Type)
            {
                case NoteType.Defend:
                {
                    Debug.Log("<color=cyan>Execute Defend and absorb some percentage of the received damage</color>");
                    
                    break;
                }
                case NoteType.Dodge:
                {
                    Debug.Log("<color=cyan>Execute Dodge</color>");
                    
                    break;
                }
            }
        }
    }
}