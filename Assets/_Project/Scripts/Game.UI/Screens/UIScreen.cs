using Game.Services;
using UnityEngine;
using System;

namespace Game.UI.Screens
{
    public abstract class UIScreen : MonoBehaviour
    {
        public event Action<UIScreen> OnClosed;
        
        private IScreenService _screenService;
        private bool _isOpen;

        public bool IsOpen => _isOpen;
        protected IScreenService ScreenService => _screenService;

        public void Close()
        {
            _isOpen = false;
            
            UnsubscribeEvents();
            
            OnClose();
        }

        protected virtual void SubscribeEvents()
        {}
        
        protected virtual void UnsubscribeEvents()
        {}

        protected virtual void OnInitialize()
        {}
        
        protected virtual void OnOpen()
        {}

        protected virtual void OnClose()
        {
            gameObject.SetActive(false);

            DispatchClosedEvent();
        }
        
        protected virtual void OnDestroy()
        {
            _screenService.UnregisterScreen(this);
        }

        protected void DispatchClosedEvent()
        {
            OnClosed?.Invoke(this);
        }
        
        private void Awake()
        {
            _screenService = ServiceLocator.GetService<IScreenService>();
            
            _screenService.RegisterScreen(this);
            
            gameObject.SetActive(false);
            
            OnInitialize();
        }

        private void OnEnable()
        {
            SubscribeEvents();
            
            OnOpen();

            _isOpen = true;
        }
    }
}
