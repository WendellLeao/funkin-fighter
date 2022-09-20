using Game.Gameplay.Playing.Automatic;
using System.Collections.Generic;
using Game.Services;
using UnityEngine;
using Game.Events;
using Game.Input;

namespace Game.Gameplay.Playing
{
    public sealed class PlayersManager : MonoBehaviour
    {
        [SerializeField] private Transform[] _spawnPoints;
        [SerializeField] private Player _playerPrefab;
        [SerializeField] private AutomaticPlayer _automaticPlayerPrefab;

        [Header("(DEBUG)")] 
        [SerializeField] private bool _spawnAutomaticPlayer;
        
        private IEventService _eventService;
        private IInputService _inputService;
        private List<PlayerBase> _activePlayers;
        private int _playersAmount = 2;

        public void Initialize(IEventService eventService)
        {
            _eventService = eventService;
            
            _inputService = ServiceLocator.GetService<IInputService>();

            _activePlayers = new List<PlayerBase>();
            
            SpawnPlayers();
        }

        public void Dispose()
        {
            foreach (PlayerBase activePlayer in _activePlayers)
            {
                activePlayer.Stop();
            }
        }

        public void Tick(float deltaTime)
        {
            foreach (PlayerBase activePlayer in _activePlayers)
            {
                activePlayer.Tick(deltaTime);
            }
        }

        private void SpawnPlayers()
        {
            for (int index = 0; index < _playersAmount; index++)
            {
                PlayerBase playerBasePrefab = GetPlayerPrefab(index);
                
                PlayerBase playerBase = Instantiate(playerBasePrefab, transform);

                playerBase.transform.position = _spawnPoints[index].position;

                FixPlayerScale(index, playerBase);

                CheckAndBeginPlayer(playerBase, index);
                
                _activePlayers.Add(playerBase);
                
                _eventService.DispatchEvent(new PlayerCreatedEvent(playerBase));
            }
        }

        private void CheckAndBeginPlayer(PlayerBase playerBase, int index)
        {
            if (playerBase is Player localPlayer)
            {
                localPlayer.Begin(_inputService, _eventService, index);
                
                return;
            }
            
            if (playerBase is AutomaticPlayer botPlayer)
            {
                botPlayer.Begin(_eventService, index);
            }
        }

        private PlayerBase GetPlayerPrefab(int index)
        {
            if (_spawnAutomaticPlayer && index > 0)
            {
                return _automaticPlayerPrefab;
            }

            return _playerPrefab;
        }

        private void FixPlayerScale(int i, PlayerBase newLocalPlayerBase)
        {
            bool isSecondPlayer = i == 1;

            if (!isSecondPlayer)
            {
                return;
            }
            
            newLocalPlayerBase.transform.localScale = new Vector3(-1, 1, 1);
        }
    }
}
