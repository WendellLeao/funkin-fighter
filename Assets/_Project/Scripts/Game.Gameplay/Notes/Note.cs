using Game.Pooling;
using DG.Tweening;
using UnityEngine;

namespace Game.Gameplay.Notes
{
    public abstract class Note : Entity
    {
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private NoteData _data;
        
        private IPoolingService _poolingService;

        public Vector2 AnchoredPosition => _rectTransform.anchoredPosition;
        public NoteData Data => _data;

        public virtual void Begin()
        {
            _rectTransform.DOMoveY(_data.EndValue, _data.Duration).OnComplete(OnComplete);
        }

        public virtual void Stop()
        { }

        public virtual void Tick(float deltaTime)
        { }

        protected abstract void Execute();
        
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