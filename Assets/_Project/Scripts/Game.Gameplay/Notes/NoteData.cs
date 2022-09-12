using Game.Pooling;
using UnityEngine;

namespace Game.Gameplay.Notes
{
    public abstract class NoteData : ScriptableObject
    {
        [Header("General Settings")]
        public NoteType Type;
        public PoolData PoolData;
        
        [Header("Notes Animation")]
        public float Duration;
        public float EndValue;

        [Header("Player Animation")] 
        public AnimationData AnimationData;
    }
}