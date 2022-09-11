using Game.Pooling;
using UnityEngine;

namespace Game.Gameplay.Notes
{
    public abstract class NoteData : ScriptableObject
    {
        [Header("General Settings")]
        public NoteType Type;
        public PoolType PoolType;
        
        [Header("Animation")]
        public float Duration;
        public float EndValue;
    }
}