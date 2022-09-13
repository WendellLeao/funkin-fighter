using Game.Services;
using UnityEngine;
using Game.Events;

namespace Game.UI
{
    public sealed class UISystem : MonoBehaviour
    {
        [SerializeField] private PlayerUIManager _playerUIManager;
        
        private void Awake()
        {
            IEventService eventService = ServiceLocator.GetService<IEventService>();
            
            _playerUIManager.Initialize(eventService);
        }

        private void OnDestroy()
        {
            _playerUIManager.Dispose();
        }

        private void Update()
        {
            _playerUIManager.Tick(Time.deltaTime);
        }
    }
}