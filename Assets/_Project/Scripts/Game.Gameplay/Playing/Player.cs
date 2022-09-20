﻿using Game.Events;
using UnityEngine;
using Game.Input;

namespace Game.Gameplay.Playing
{
    public sealed class Player : PlayerBase
    {
        [SerializeField] private NotesExecutor _notesExecutor;

        public override void Stop()
        {
            base.Stop();
            
            _notesExecutor.Dispose();
        }

        public override void Tick(float deltaTime)
        {
            base.Tick(deltaTime);
            
            _notesExecutor.Tick(deltaTime);
        }
        
        public void Begin(IInputService inputService, IEventService eventService, int index)
        {
            base.Begin(_notesExecutor, eventService, index);
            
            NotesExecutor = _notesExecutor;

            _notesExecutor.Initialize(inputService, EventService, index);
        }
    }
}
