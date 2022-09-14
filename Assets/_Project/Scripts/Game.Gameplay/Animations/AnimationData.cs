using UnityEngine;

namespace Game.Gameplay.Animations
{
    [CreateAssetMenu(menuName = "Animation Data/AnimData", fileName = "AnimData")]
    public sealed class AnimationData : ScriptableObject
    {
        public string ID;
    }
}