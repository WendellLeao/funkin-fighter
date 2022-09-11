using Game.Gameplay.Playing;
using Game.Gameplay.Notes;
using UnityEngine;

namespace Game.Gameplay
{
    public sealed class GameplaySystem : MonoBehaviour
    {
        [SerializeField] private PlayerManager _playerManager;
        [SerializeField] private NotesManager _notesManager;

        private void Awake()
        {
            _playerManager.Initialize();
            _notesManager.Initialize(_playerManager.Player);//TODO: Set the executor dynamically
        }

        private void OnDestroy()
        {
            _playerManager.Dispose();
            _notesManager.Dispose();
        }

        private void Update()
        {
            _playerManager.Tick(Time.deltaTime);
            _notesManager.Tick(Time.deltaTime);
        }
    }
}
