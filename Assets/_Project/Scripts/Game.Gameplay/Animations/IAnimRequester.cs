﻿using System;

namespace Game.Gameplay.Animations
{
    public interface IAnimRequester
    {
        public event Action<string, bool> OnAnimateBool;
        public event Action<string> OnAnimateTrigger;
    }
}