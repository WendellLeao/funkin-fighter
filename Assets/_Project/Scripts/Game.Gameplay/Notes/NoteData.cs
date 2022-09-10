using Game.Pooling;
using UnityEngine;

namespace Game.Gameplay.Notes
{
    [CreateAssetMenu(menuName = "Playing/NoteData", fileName = "NoteData")]
    public class NoteData : ScriptableObject
    {
        public NoteType Type;
        public PoolType PoolType;
        public float Duration;
        public float EndValue;
    }
}