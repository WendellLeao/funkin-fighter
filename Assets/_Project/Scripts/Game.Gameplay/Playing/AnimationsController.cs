﻿using UnityEngine;

namespace Game.Gameplay.Playing
{
    public sealed class AnimationsController : MonoBehaviour
    {
        [SerializeField] private Animator _animator;

        private IAnimRequester[] _requesters;

        public void Initialize()
        {
            _requesters = gameObject.GetComponents<IAnimRequester>();

            foreach (var animRequester in _requesters)
            {
                animRequester.OnAnimateTrigger += HandleAnimationTrigger;
                animRequester.OnAnimateBool += HandleAnimationBool;
            }
        }

        public void Dispose()
        {
            foreach (var animRequester in _requesters)
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