using Game.Pooling;
using DG.Tweening;
using UnityEngine;
using System;

namespace Game.Gameplay.Notes
{
    public abstract class Note : Entity
    {
        public event Action<Note> OnNoteExecuted;

        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private NoteData _data;
        
        private IPoolingService _poolingService;
        private bool _hasEnteredExecutionArea;
        private bool _hasExecuted;

        public NoteData Data => _data;
        public Vector2 AnchoredPosition => _rectTransform.anchoredPosition;
        public bool HasEnteredExecutionArea => _hasEnteredExecutionArea;
        public bool HasExecuted => _hasExecuted;

        public void Begin(IPoolingService poolingService)
        {
            _poolingService = poolingService;
            
            _hasEnteredExecutionArea = false;
            _hasExecuted = false;
            
            // _rectTransform.DOMoveY(_data.EndValue, _data.Duration).OnComplete(OnComplete);
            _rectTransform.DOMoveY(_data.EndValue, _data.Duration);
        }

        public virtual void Stop()
        { }

        public virtual void Tick(float deltaTime)
        { }

        public virtual void EnterExecutionArea()
        {
            _hasEnteredExecutionArea = true;
        }
        
        public virtual void ExitExecutionArea()
        {
            _hasEnteredExecutionArea = false;
        }
        
        public virtual void Execute(bool hasCorrectlyHit)
        {
            _hasExecuted = true;
            
            OnNoteExecuted?.Invoke(this);
            
            Destroy(gameObject);
        }
        
        protected virtual void OnComplete()
        {
            _poolingService.ReturnObjectToPool(_data.PoolType, gameObject);
        }
        
        public void SetPosition(Vector3 position)
        {
            _rectTransform.position = position;
        }
    }
}