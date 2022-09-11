using UnityEngine;

namespace Game.UI
{
    public sealed class UISystem : MonoBehaviour
    {
        [SerializeField] private PlayerUIManager _playerUIManager;
        
        private void Awake()
        {
            _playerUIManager.Initialize();
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