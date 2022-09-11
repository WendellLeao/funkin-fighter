using Game.UI.Playing;
using UnityEngine;

namespace Game.UI
{
    public sealed class PlayerUIManager : MonoBehaviour
    {
        [SerializeField] private PlayerStatusUI _playerStatusUI;

        public void Initialize()
        {
            _playerStatusUI.Begin();
        }

        public void Dispose()
        {
            _playerStatusUI.Stop();
        }

        public void Tick(float deltaTime)
        {
            _playerStatusUI.Tick(deltaTime);
        }
    }
}