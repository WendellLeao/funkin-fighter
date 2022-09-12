using UnityEngine;

namespace Game.Gameplay.Notes
{
    [CreateAssetMenu(menuName = "Animation/AnimationData", fileName = "AnimationData")]
    public sealed class AnimationData : ScriptableObject
    {
        public string ID;
    }
}