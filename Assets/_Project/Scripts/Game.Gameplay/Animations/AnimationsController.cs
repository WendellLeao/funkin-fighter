using UnityEngine;

namespace Game.Gameplay.Animations
{
    public sealed class AnimationsController : MonoBehaviour
    {
        [SerializeField] private Animator _animator;

        private IAnimRequester[] _requesters;

        public void Initialize()
        {
            _requesters = GetComponentsInParent<IAnimRequester>();

            foreach (IAnimRequester animRequester in _requesters)
            {
                animRequester.OnAnimateTrigger += HandleAnimationTrigger;
                animRequester.OnAnimateBool += HandleAnimationBool;
            }
        }

        public void Dispose()
        {
            foreach (IAnimRequester animRequester in _requesters)
            {
                animRequester.OnAnimateTrigger -= HandleAnimationTrigger;
                animRequester.OnAnimateBool -= HandleAnimationBool;
            }
        }

        private void HandleAnimationTrigger(string animationHash)
        {
            _animator.SetTrigger(animationHash);
        }
        
        private void HandleAnimationBool(string animationHash, bool value)
        {
            _animator.SetBool(animationHash, value);
        }
    }
}