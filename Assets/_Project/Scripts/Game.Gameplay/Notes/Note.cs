using Game.Gameplay.Playing;
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
        private INotesExecutor _notesExecutor;
        private bool _hasEnteredExecutionArea;
        private bool _hasExecuted;

        public NoteData Data => _data;
        public INotesExecutor NotesExecutor => _notesExecutor;
        public Vector2 AnchoredPosition => _rectTransform.anchoredPosition;
        public bool HasEnteredExecutionArea => _hasEnteredExecutionArea;
        public bool HasExecuted => _hasExecuted;

        public void Begin(IPoolingService poolingService)
        {
            _poolingService = poolingService;

            // _rectTransform.DOMoveY(_data.EndValue, _data.Duration).OnComplete(OnComplete);
            _rectTransform.DOMoveY(_data.EndValue, _data.Duration);
        }

        public virtual void Stop()
        {
            // _hasEnteredExecutionArea = false;//TODO: Review this when implement pooling
            // _hasExecuted = false;
            //
            // _notesExecutor = null;
        }

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
        
        public virtual void Execute( bool hasCorrectlyHit)
        {
            _hasExecuted = true;

            if (!hasCorrectlyHit)
            {
                return;
            }

            OnNoteExecuted?.Invoke(this);
            
            Destroy(gameObject);
        }
        
        protected virtual void OnComplete()
        {
            PoolData poolData = _data.PoolData;
            
            _poolingService.ReturnObjectToPool(poolData.PoolType, gameObject);
        }

        public void SetExecutor(INotesExecutor notesExecutor)
        {
            _notesExecutor = notesExecutor;
        }
        
        public void SetPosition(Vector3 position)
        {
            _rectTransform.position = position;
        }
    }
}